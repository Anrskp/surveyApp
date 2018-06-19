# Install node env
FROM node:8

RUN apt-get update
RUN apt-get install libcairo2-dev libpango1.0-dev libgif-dev libjpeg62-turbo-dev build-essential g++ -y


# Create app directory
WORKDIR /graphService

# Install app dependencies
# A wildcard is used to ensure both package.json AND package-lock.json are copied
# where available (npm@5+)

COPY package*.json ./
#ADD . /graphService
#RUN cd /graphService && npm install


# If you are building your code for production
# RUN npm install --only=production
RUN npm install



# Bundle app source
COPY . .

# Open port
EXPOSE 9000


CMD [ "npm", "start" ]
