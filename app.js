// Imports
const express = require('express');
const mongoose = require('mongoose');
const path = require('path');
const bodyParser = require('body-parser');
const cors = require('cors');
const config = require('./config/config');
const User = require('./models/user');
const SR = require('./routes/surveyRoute');

// DATABASE CONNECTION

// Promise libary
mongoose.Promise = require('bluebird');

// Connect To Database
mongoose.connect(config.database);

// On Connection
mongoose.connection.on('connected', () => {
  console.log('Connected to database ' + config.database)
})

// On Error
mongoose.connection.on('error', (err) => {
  console.log('Database error : ' + err);
});

// Declare express variable
const app = express();

// Set Static Folder

// CORS Middleware
app.use(cors());

//Body Parser Middleware
app.use(bodyParser.json());

// Routes

// Set port number
const port = process.env.PORT || 3000;

// Start server
app.listen(port, () => {
  console.log('Server startet on port ' + port);
});

let mySurvey =   {
    "Title":"John's Survey",
    "Desc" : "A Survey about john",
    "Author":"John Wick",
    "Questions": [
      { "Question":"how old is john",
      "Answers":[ "20yo", "25yo", "30yo" ]},
      { "Question":"which gun does john use",
      "Answers":[ "magnum", "9mm", "desert eagle" ]},
      { "Question":"which car does john drive",
      "Answers":[ "Fiat Punto", "mazda 3", "Volvo" ]}
    ]
 }
// retrive from json example
/*
console.log(mySurvey.Questions[0].Question)
let answers = ((mySurvey.Questions[0].Answers))
answers.forEach( e => console.log(e));
*/
SR.sendSomething(mySurvey);
