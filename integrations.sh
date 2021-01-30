#!/bin/bash

KUBE_CONTEXT=minikube
NAMESPACE=dmt


create-dotnet-image() {
        docker build . -t dotnetcollection:latest -f RealWear.DeviceManagement/Dockerfile
        }

delete-recreate-ns() {
        kubectl delete namespace --ignore-not-found=true $NAMESPACE --context=$KUBE_CONTEXT
        kubectl create namespace $NAMESPACE --context=$KUBE_CONTEXT
        }


create-mogno-db() {
        kubectl delete pod/realweardb --ignore-not-found=true -n $NAMESPACE --context=$KUBE_CONTEXT
        kubectl delete service/realweardb --ignore-not-found=true -n $NAMESPACE --context=$KUBE_CONTEXT
        kubectl create -f mongo_create.yaml --context=$KUBE_CONTEXT
        kubectl create -f mongo_service.yaml --context=$KUBE_CONTEXT
        }

# WAIT FOR MONGO POD AND SERVICE
check-if-mongo-ready() {
        kubectl wait --for=condition=ready pod/realweardb -n $NAMESPACE --context=$KUBE_CONTEXT
        kubectl wait --for=condition=ready service/realweardb -n $NAMESPACE --context=$KUBE_CONTEXT
        }

create-dotnet-pod() {
        kubectl delete pod/dotnet --ignore-not-found=true -n $NAMESPACE --context=$KUBE_CONTEXT
        kubectl delete service/dotnet --ignore-not-found=true -n $NAMESPACE --context=$KUBE_CONTEXT
        kubectl create -f asp_pod.yaml --context=$KUBE_CONTEXT
        kubectl create -f asp_service.yaml --context=$KUBE_CONTEXT
        }

check-if-dotnet-ready() {
        kubectl wait --for=condition=ready pod/dotnet -n $NAMESPACE --context=$KUBE_CONTEXT
        kubectl wait --for=condition=ready service/dotnet -n $NAMESPACE --context=$KUBE_CONTEXT
        }


run-test() {
                docker build . -t runcollections:latest -f DockerfileHarness
                kubectl run newman --restart=Never --image=runcollections:latest --namespace $NAMESPACE --image-pull-policy=IfNotPresent --attach --rm -- run "RealWearDeviceManagementService_IntegrationTest23.postman_collection"
		}
	


create-dotnet-image
delete-recreate-ns
create-mogno-db
check-if-mongo-ready
create-dotnet-pod
check-if-dotnet-ready
run-test


