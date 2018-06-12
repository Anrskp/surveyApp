"use strict";

const express = require('express');
const passport = require('passport');
const surveyCRUD = require('../rabbitMQ/surveyCRUD');
const router = express.Router();

/*

 TODO : insert Passport Authenticate Middleware  < passport.authenticate('jwt', { session: false }) >
        check and foward messages from .NET service

*/


// Create a survey
router.post('/createNewSurvey', (req, res, next) => {
  try {
    let newSurvey = JSON.parse(req.body);
    surveyCRUD.RPC(newSurvey, 'rpc_save_survey').then(x => {
      res.json({
        success: true,
        data: x
      });
    })
  } catch (e) {
    res.json({
      success: false,
      msg: 'Something went wrong'
    })
  }
});

// Get surveys overview (id, name, author, desc)
router.post('/getSurveys', (req, res, next) => {
  try {
    surveyCRUD.RPC(req.body.userID, 'rpc_return_surveys_unpop').then(x => {
      res.json({
        success: true,
        survey: x
      })
    })
  } catch (e) {
    res.json({
      success: false,
      msg: 'Something went wrong'
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


// Send in answers for specific survey
router.post('/sendSurveyAnswers', (req, res, next) => {

  let answers = JSON.stringify(req.body);

  try {
    surveyCRUD.RPC(answers, 'rpc_save_answers').then(x => {
        console.log(x)

        if (x.success == true) {

          res.json({
            success: true,
            msg: 'Your answers have been saved! thanks for participating'
          })
        } else {
          res.json({
            success: false,
            msg: 'Something went wrong try again later'
          })
        }
      })
    }
    catch (e) {
      console.error(e);
      res.json({
        success: false,
        msg: 'Something went wrong!'
      })
    }

})

// Get survey data
router.post('/getSurveyData', (req, res, next) => {
  const surveyID = req.body;

  try {
    surveyCRUD.RPC(surveyID, 'que_name').then(x => {
      res.json({
        success: true,
        survey: x
      })
    })
  } catch (e) {
    console.error(e);
    res.json({
      success: false,
      msg: 'Something went wrong!'
    })
  }
});

// Delete survey
router.post('/deleteSurveyByID', (req, res, next) => {

  const surveyToDelete = req.body;

  try {
    surveyCRUD.RPC(surveyToDelete, 'que_name').then(x => {
      res.json({
        success: true,
        msg: 'Survey deleted!'
      })
    })
  } catch (e) {
    console.error(e);
    res.json({
      success: false,
      msg: 'Something went wrong!'
    })
  }
});

// send email notification with survey link
router.post('/sendEmailNotification', (req, res, next) => {
  // TODO
});

module.exports = router;
