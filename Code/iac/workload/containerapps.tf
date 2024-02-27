resource "azurerm_user_assigned_identity" "acapull" {
  name                = "id-aca-app"
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
}

resource "azurerm_role_assignment" "acrpull" {
  scope                = data.azurerm_container_registry.acr.id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_user_assigned_identity.acapull.principal_id
}

resource "azurerm_container_app_environment" "acaenv" {
  name                       = "acaenv-openai"
  resource_group_name        = data.azurerm_resource_group.rg.name
  location                   = data.azurerm_resource_group.rg.location
  infrastructure_subnet_id   = azurerm_subnet.apps.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.law.id
}

output "app_url" {
  value = azurerm_container_app.app.ingress[0].fqdn
}

resource "azurerm_container_app" "app" {
  name                         = "app"
  container_app_environment_id = azurerm_container_app_environment.acaenv.id
  resource_group_name          = data.azurerm_resource_group.rg.name
  revision_mode                = "Single"
  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.acapull.id]
  }
  registry {
    server   = data.azurerm_container_registry.acr.login_server
    identity = azurerm_user_assigned_identity.acapull.id
  }
  ingress {
    external_enabled = true
    target_port      = 8000
    traffic_weight {

      latest_revision = true
      percentage      = 100
    }
  }
  secret {
    name  = "azure-open-ai-key"
    value = azurerm_cognitive_account.openai.primary_access_key
  }
  template {

    container {
      name  = "app"
      image = "${data.azurerm_container_registry.acr.name}.azurecr.io/app:${var.image_tag}"

      cpu    = 1
      memory = "2Gi"
      env {
        name  = "ASPNETCORE_URLS"
        value = "http://+:8000"
      }
      env {
        name  = "OPENAIACCESS__TYPE"
        value = "AzureOpenAi"
      }
      env {
        name        = "OPENAIACCESS__APIKEY"
        secret_name = "azure-open-ai-key"
      }
      env {
        name  = "OPENAIACCESS__AZUREOPENAIENDPOINT"
        value = "https://${azurerm_cognitive_account.openai.custom_subdomain_name}.openai.azure.com"
      }
      env {
        name  = "OPENAIACCESS__MODELNAME"
        value = azurerm_cognitive_deployment.gpt4.name
      }
      readiness_probe {
        transport = "HTTP"
        port      = 8000
        path      = "/health/readiness"
      }
      liveness_probe {
        transport        = "HTTP"
        port             = 8000
        path             = "/health/liveness"
        initial_delay    = 20
        interval_seconds = 30
      }
    }
  }
}
