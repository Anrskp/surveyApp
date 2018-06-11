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
    surveyCRUD.createNewSurvey(req.body).then(x => {
      res.json({
        success:true,
        data: x
      });
    })
  } catch (e) {
    console.log(e);
    res.json({
      success : false,
      msg:'Something went wrong'
    })
  }
});

// Get surveys overview
router.post('/getSurveys', (req, res, next) => {

  try {
    surveyCRUD.getSurvey(req.body.userID).then(x => {
      //res.send(x)
      res.json({
        success : true,
        survey : x
      })
    })
  } catch (e) {
    res.json({
      success : false,
      msg:'Something went wrong'
    })
  }
});

// Get survey data


// Delete survey


// Get a specific survey
router.post('/getSurveyByID', (req, res, next) => {
  // todo
})

// Send in answers for specific survey


// send email notification with survey link

module.exports = router;
