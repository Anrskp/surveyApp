const express = require('express');
const router = express.Router();
const passport = require('passport');
const surveyCRUD = require('../rabbitMQ/surveyCRUD');

// Example survey
let mySurvey = {
  "Title": "John's Survey",
  "Desc": "A Survey about john",
  "Author": "John Wick",
  "Questions": [{
      "Question": "how old is john",
      "Answers": ["20yo", "25yo", "30yo"]
    },
    {
      "Question": "FEAR OF THE DARK",
      "Answers": ["magnum", "9mm", "desert eagle"]
    },
    {
      "Question": "which car does john drive",
      "Answers": ["Fiat Punto", "mazda 3", "Volvo"]
    }
  ]
}

// Create survey
router.get('/sendSurvey', (req, res, next) => {
  surveyCRUD.sendSurvey(mySurvey).then(x => {
    console.log(x);
  })
});

// Get surveys overview
// Get survey data
// Delete survey

// Get a specific survey
// Send in answers for specific survey

// send email notification with survey link

module.exports = router;
