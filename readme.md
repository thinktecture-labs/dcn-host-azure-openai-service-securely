# Host Azure OpenAI Service securely with Terraform - Digital Craftsmanship Nordoberpfalz February 2024

This repo contains the code examples and slides for the "Host Azure OpenAI securely with Terraform" talk which was held at the DCN by Kenny Pflug on February 27th 2024.

## How to run the example

Please make sure that you have the following prerequisites installed and configured:

- a bash with [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-linux?pivots=apt) and [Terraform](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli)
- please make sure that you are logged in to Azure and that the correct subscription is selected

```bash
# Log in to Azure via the CLI 
az login
# Show the credentials that are currently active (account + selected subscriptions)
az account show
# Show all available subscriptions
az account list --output table
# Set the currently active subscription - replace subscription-id with the desired value from the table
az account set --subscription subscription-id
```

You can then deploy the whole solution by executing:

```bash
cd Code
./deploy.sh
```

This will perform the following three tasks:

1. Deploy the "global" terraform project (creates a resource group and container registry)
1. Uploads the source code to the container registry which then builds the docker image
1. Deploy the "workload" terraform project

This will take roughly 10 minutes. The deploy script will return the URL of the Azure Container App.
