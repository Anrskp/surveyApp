const express = require('express');
const router = express.Router();
const passport = require('passport');
const surveyCRUD = require('../rabbitMQ/surveyCRUD');

// Create a survey
router.post('/createNewSurvey', (req, res, next) => {
  try {
    surveyCRUD.RPC(req.body, 'rpc_save_survey').then(x => {
      res.json({
        success:true,
        data: x
      });
    })
  } catch (e) {
    res.json({
      success : false,
      msg:'Something went wrong'
    })
  }
});

// Get surveys overview (id, name, author, desc)
router.post('/getSurveys', (req, res, next) => {
  try {
    surveyCRUD.RPC(req.body.userID, 'rpc_return_surveys_unpop').then(x => {
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

// Get a specific survey
router.post('/getSurveyByID', (req, res, next) => {
  try {
    surveyCRUD.RPC(req.body.surveyID, 'rpc_single_survey').then(x => {
      res.json({
        success: true,
        survey: x
      })
    })
  } catch (e) {
    console.error(e);
    res.json({
      success: false,
      msg: 'Something went wrong'
    })
  }
})

// Get survey data
router.post('/getSurveyData', (req, res, next) => {
  // todo
})


// Delete survey
router.post('/deleteSurveyByID', (req, res, next) => {
  // todo
})


// Send in answers for specific survey


// send email notification with survey link

module.exports = router;

// Example survey
let testSurvey = {
  "Title": "John's Survey",
  "Desc": "A Survey about john",
  "Author": "123131313", // user ID
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
