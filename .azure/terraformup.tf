# housingxyz :: terraform

## backends
terraform {
  backend "azurerm" {
    resource_group_name = "revaturexyzgroup"
    storage_account_name = "revaturexyzaccount"
    container_name = "terraformxyz"
    key = "housingxyz.tfstate"
  }
}

## providers
provider "azuread" {
  version = "~>0.4.0"
}

provider "azurerm" {
  version = "~>1.30.0"
}

provider "random" {
  version = "~>2.1.0"
}

## resources
resource "azurerm_resource_group" "housingxyz" {
  name = "housingxyzgroup"
  location = "southcentralus"
  tags = {
    owner = "fred belotte"
  }
}

resource "azurerm_app_service_plan" "housingxyz" {
  kind = "Linux"
  location = "${azurerm_resource_group.housingxyz.location}"
  name = "housingxyzplan"
  resource_group_name = "${azurerm_resource_group.housingxyz.name}"
  reserved = true

  sku {
    size = "B1"
    tier = "Basic"
  }
}
