# vars

DOCKER_NAMESPACE=addcode-practicum
DOCKER_IMAGE_API=$(DOCKER_NAMESPACE):video-chat-api
DOCKER_IMAGE_PROXY=$(DOCKER_NAMESPACE):video-chat-proxy
DOCKER_CONTAINER_NAME=video-chat-api
DOCKER_HOST=ssh://$(user)@$(ip)

# build

build:
	docker build -f deploy/Dockerfile -t $(DOCKER_IMAGE_API) .
	docker build -f deploy/nginx/Dockerfile -t $(DOCKER_IMAGE_PROXY) .

# start

start:
	docker run -d --name $(DOCKER_CONTAINER_NAME) -p 5000:5000 $(DOCKER_IMAGE)

# clear

clear:
	docker stop $(DOCKER_CONTAINER_NAME)
	docker rm $(DOCKER_CONTAINER_NAME)

# deploy

deployment:
	DOCKER_HOST=ssh://$(user)@$(ip) docker build -f deploy/Dockerfile -t $(DOCKER_IMAGE_API) .
	DOCKER_HOST=ssh://$(user)@$(ip) docker build -f deploy/nginx/Dockerfile -t $(DOCKER_IMAGE_PROXY) .
	DOCKER_HOST=ssh://$(user)@$(ip) docker-compose -f deploy/docker-compose.yml stop
	DOCKER_HOST=ssh://$(user)@$(ip) docker-compose -f deploy/docker-compose.yml rm -f
	DOCKER_HOST=ssh://$(user)@$(ip) docker-compose -f deploy/docker-compose.yml up -d
