#! /bin/bash

# Exit on error
set -e

if ! command -v terraform &> /dev/null
then
    echo "Terraform could not be found"
    exit
fi

if ! command -v az &> /dev/null
then
    echo "Azure CLI could not be found"
    exit
fi

echo "Deploying Azure Container Registry..."

cd iac
cd global

terraform init -upgrade
terraform validate
terraform apply -auto-approve

rgName=$(terraform output -raw resource_group_name)
acrName=$(terraform output -raw acr_name)
suffix=$(terraform output -raw suffix)

echo "Created Resource Group:           $rgName"
echo "Created Azure Container Registry: $acrName"
echo "Created Resource Suffix:          $suffix"

cd ..
cd ..
echo "Azure Container Registry is deployed"
read -p "Press return to continue"
imageTag="0.0.1"
# call build_app_image only if --skip-build flag is not set
if [ "$1" != "--skip-build" ]; then
    echo "Building and Pushing Application Image to Azure Container Registry"
    az acr login --name $acrName
    imageTag=$(az acr build --no-logs -r $acrName -t app:{{.Run.ID}} . -otsv --query "runId")
    echo "Container Image build and pushed to Azure Container Registry"
    read -p "Press return to continue"
else
    imageTag=$(az acr repository show-tags -n $acrName --repository app --orderby time_desc -otsv --query [0])
fi

echo "Using Image Tag:               $imageTag"
echo "Deploying Application Infrastructure"

cd iac
cd workload

terraform init -upgrade
terraform validate
terraform apply -var resource_group_name=$rgName -var acr_name=$acrName -var image_tag=$imageTag -var suffix=$suffix -auto-approve

echo "Application Infrastructure Deployed"
echo "✅ All Done!"
