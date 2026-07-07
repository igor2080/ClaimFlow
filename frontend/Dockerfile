FROM node:20-alpine

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

#Vite's port
EXPOSE 5173

CMD ["npm", "run", "dev", "--", "--host"]