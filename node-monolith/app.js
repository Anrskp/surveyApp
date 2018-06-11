// Imports
const express = require('express');
const mongoose = require('mongoose');
const path = require('path');
const cors = require('cors');
const bodyParser = require('body-parser');
const passport = require('passport');
const config = require('./config/config');
const user = require('./routes/userRoutes');
const survey = require('./routes/surveyRoutes');

// DATABASE CONNECTION
// Promise libary
mongoose.Promise = require('bluebird');

// Connect To Database
mongoose.connect(config.database);

// On Connection
mongoose.connection.on('connected', () => {
  console.log('Connected to database ' + config.database)
});

// On Error
mongoose.connection.on('error', (err) => {
  console.log('Database error : ' + err);
});

// Declare express variable
const app = express();

// Set Static Folder
// <insert angular static folder here>
app.use(express.static(path.join(__dirname, './angular-src/dist')));
// CORS Middleware
app.use(cors());

//Body Parser Middleware
app.use(bodyParser.json());

// Passport Middleware
app.use(passport.initialize());
app.use(passport.session());
require('./config/passport')(passport);

// Routes
app.use('/users', user);
app.use('/survey', survey);

// Set port number
const port = process.env.PORT || 5000;

// Start server
app.listen(port, () => {
  console.log('Server startet on port ' + port);
});


// Testing RabbitMQ
//const surveyCRUD = require('./rabbitMQ/surveyCRUD');

//surveyCRUD.getSurvey('John Wick').then(x => console.log(x));
