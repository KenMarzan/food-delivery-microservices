FROM node:20-alpine
WORKDIR /app
RUN echo '{"name":"food-delivery-demo","version":"1.0.0"}' > package.json
COPY . .
CMD ["node", "-e", "console.log('food-delivery-microservices demo container running'); setInterval(()=>{},1000*60*60)"]
