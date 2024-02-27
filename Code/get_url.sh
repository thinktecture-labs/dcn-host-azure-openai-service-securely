#!/bin/bash

set -e

cd iac/workload
terraform output -raw app_url
