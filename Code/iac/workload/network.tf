resource "azurerm_virtual_network" "vnet" {
  name                = "vnet-openai-${var.suffix}"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  address_space       = ["10.0.0.0/8"]
}

resource "azurerm_subnet" "apps" {
  name                 = "sn-apps"
  virtual_network_name = azurerm_virtual_network.vnet.name
  resource_group_name  = data.azurerm_resource_group.rg.name
  address_prefixes     = ["10.1.0.0/16"]
}

resource "azurerm_subnet" "backend" {
  name                 = "sn-backend"
  virtual_network_name = azurerm_virtual_network.vnet.name
  resource_group_name  = data.azurerm_resource_group.rg.name
  address_prefixes     = ["10.2.0.0/16"]
}

resource "azurerm_subnet_network_security_group_association" "backend" {
  subnet_id                 = azurerm_subnet.backend.id
  network_security_group_id = azurerm_network_security_group.backend.id
}

resource "azurerm_network_security_group" "backend" {
  name                = "nsg-backend"
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location

  security_rule {
    name        = "allow-apps-to-openai-https"
    description = "Allow HTTPS from the apps subnet"
    access      = "Allow"
    direction   = "Inbound"
    priority    = 150
    protocol    = "Tcp"

    source_address_prefix = azurerm_subnet.apps.address_prefixes[0]
    source_port_range     = "*"

    destination_port_range     = "443"
    destination_address_prefix = "${azurerm_private_endpoint.endpoint.private_service_connection.0.private_ip_address}/32"
  }

  security_rule {
    name        = "prevent-all"
    description = "Deny all inbound traffic"
    direction   = "Inbound"
    access      = "Deny"
    priority    = 200
    protocol    = "Tcp"

    source_address_prefix = "*"
    source_port_range     = "*"

    destination_port_range     = "443"
    destination_address_prefix = azurerm_subnet.backend.address_prefixes[0]
  }
}
