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
router.post('/createNewSurvey', (req, res, next) => {
  try {
    surveyCRUD.createNewSurve(req.body).then(x => {
      res.json({
        data: x
      });
    })
  } catch (e) {
    res.json({
      error: 'there was an error! try again later'
    })
  }
});

// Get surveys overview
router.post('/getSurveys', (req, res, next) => {
  try {
    surveyCRUD.getSurvey(req.body).then(x => {
      res.json({
        data: x
      })
    })
  } catch (e) {
    res.json({
      error: 'there was an error! try again later'
    })
  }
})

// Get survey data


// Delete survey


// Get a specific survey
router.post('/getSurveyByID', (req, res, next) => {
  // todo
})

// Send in answers for specific survey


// send email notification with survey link

module.exports = router;
