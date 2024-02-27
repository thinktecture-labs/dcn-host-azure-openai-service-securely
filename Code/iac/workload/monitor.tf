resource "azurerm_log_analytics_workspace" "law" {
  name                = "law-kp"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  sku                 = "PerGB2018"
}
