# vars

DOCKER_NAMESPACE=addcode-practicum
DOCKER_IMAGE=$(DOCKER_NAMESPACE):video-chat-backend
DOCKER_CONTAINER_NAME=video-chat-backend
DOCKER_HOST=ssh://$(user)@$(ip)

# build

build:
	docker build -t $(DOCKER_IMAGE) .

# start

start:
	docker run -d --name $(DOCKER_CONTAINER_NAME) -p 5000:5000 $(DOCKER_IMAGE)

# clear

clear:
	docker stop $(DOCKER_CONTAINER_NAME)
	docker rm $(DOCKER_CONTAINER_NAME)

# deploy

deployment:
	DOCKER_HOST=ssh://$(user)@$(ip) docker build -t $(DOCKER_IMAGE) .
	DOCKER_HOST=ssh://$(user)@$(ip) docker stop $(DOCKER_CONTAINER_NAME)
	DOCKER_HOST=ssh://$(user)@$(ip) docker rm $(DOCKER_CONTAINER_NAME)
	DOCKER_HOST=ssh://$(user)@$(ip) docker run -d --name $(DOCKER_CONTAINER_NAME) -p 5000:5000 $(DOCKER_IMAGE)
