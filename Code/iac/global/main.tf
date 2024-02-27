terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.91.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "3.5.1"
    }

  }
}

provider "azurerm" {
  features {}
}

provider "random" {

}
resource "random_integer" "suffix" {
  min = 1000
  max = 9999
}

resource "azurerm_resource_group" "rg" {
  name     = "rg-openai-${random_integer.suffix.result}"
  location = var.location
}

resource "azurerm_container_registry" "acr" {
  name                = "azopenai${random_integer.suffix.result}"
  admin_enabled       = false
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "Standard"
}

output "suffix" {
  value = random_integer.suffix.result
}

output "acr_name" {
  value = azurerm_container_registry.acr.name
}

output "resource_group_name" {
  value = azurerm_resource_group.rg.name
}

