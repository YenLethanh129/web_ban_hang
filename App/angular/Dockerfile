# Stage 1: Build the Angular app
FROM node:20-alpine3.19 AS build

COPY dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
EXPOSE 4200
CMD ["npm", "start"]