FROM nginx:stable-alpine

COPY deploy/nginx/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 4000
CMD ["nginx", "-g", "daemon off;"]
