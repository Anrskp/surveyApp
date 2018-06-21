"use strict";

const express = require('express');
const passport = require('passport');
const surveyCRUD = require('../rabbitMQ/surveyCRUD');
const router = express.Router();
const fs = require('fs');


// Create a survey
router.post('/createNewSurvey', (req, res, next) => {
  try {
    let newSurvey = JSON.stringify(req.body);

    surveyCRUD.RPC(newSurvey, 'rpc_save_survey').then(x => {

      let reply = JSON.parse(x);

      if (reply.success) {
        res.json({
          success: true,
        });
      } else {
        res.json({
          success: false,
          msg: 'Something went wrong'
        });
      }

    })
  } catch (e) {
    console.log(e);
    res.json({
      success: false,
      msg: 'Something went wrong'
    })
  }
});

// Get surveys overview (id, name, author, desc)
router.post('/getSurveys', (req, res, next) => {
  try {
    let surveyID = (req.body.userID);
    console.log('Survey id' + surveyID);

    surveyCRUD.RPC(surveyID, 'rpc_return_surveys_unpop').then(x => {
      console.log(x);
      let reply = JSON.parse(x);

      if (!reply.success) {
        res.json({
          success: false,
          msg: 'Something went wrong'
        });
      } else {
        res.json({
          success: true,
          survey: JSON.stringify(reply.body)
        })
      }
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
      let reply = JSON.parse(x);

      if (reply.success) {
        res.json({
          success: true,
          survey: JSON.stringify(reply.body)
        })
      } else {
        res.json({
          success: false,
          msg: 'Something went wrong'
        })
      }
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

      let reply = JSON.parse(x);

      if (reply.success) {

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
  } catch (e) {
    console.error(e);
    res.json({
      success: false,
      msg: 'Something went wrong!'
    })
  }

})


// Delete survey
router.post('/deleteSurveyByID', (req, res, next) => {

  const surveyToDelete = req.body.surveyID;

  try {
    surveyCRUD.RPC(surveyToDelete, 'rpc_delete_survey').then(x => {

      let reply = JSON.parse(x)

      if (reply.success) {
        res.json({
          success: true,
          msg: 'Survey deleted!'
        })
      } else {
        res.json({
          success: false,
          msg: 'Something went wrong!'
        })
      }
    })
  } catch (e) {
    console.error(e);
    res.json({
      success: false,
      msg: 'Something went wrong!'
    })
  }
});

// Get survey data
router.post('/getSurveyData', (req, res, next) => {

  const surveyID = (req.body.surveyID);

  try {
    surveyCRUD.RPC(surveyID, 'rpc_survey_results').then(results => {

      let surveyInfo = []

      let res = JSON.parse(results);

      let questionAmount = res.body.length;
      for (let i = 0; i < questionAmount; i++) {

        let surveyObj = {
          "questionTxt": res.body[i].QuestionText,
          "url": res.body[i].QID
        };

        surveyInfo.push(surveyObj);

        surveyCRUD.RPC(JSON.stringify(res.body[i]), 'rpc_gen_graph').then(data => {
          var buf = Buffer.from(data, 'base64');

          fs.createWriteStream("images/" + res.body[i].QID + ".png").write(buf);
          fs.createWriteStream("images/" + res.body[i].QID + ".png").end();
        })
      }
        return surveyInfo;
    }).then(x => {
      res.json({
        success: true,
        body: x
      })
    });
  } catch (e) {
    console.error(e);
    res.json({
      success: false,
      msg: 'Something went wrong!'
    })
  }
});

/*
router.get('/generateGraph', (req, res, next) => {

  console.log(req.body);

  let questionID = 'testGraph';
  let questionLabels = ['ad', 'ad', 'ad'];
  let questionData = [12, 43, 21];

  try {
    surveyCRUD.RPC('test', 'rpc_gen_graph').then(data => {
      var buf = Buffer.from(data, 'base64');

      fs.createWriteStream("images/" + questionID + ".png").write(buf);
      fs.createWriteStream("images/" + questionID + ".png").end();

      res.json({
        success: true
      })

    })
  } catch (e) {
    res.json({
      success: false,
      msg: 'Something went wrong!'
    });
  }
})
*/

// send email notification with survey link
router.post('/sendEmailNotification', (req, res, next) => {
  try {
    let newEmail = JSON.stringify(req.body);

    surveyCRUD.RPC(newEmail, 'emailQueue').then(x => {

      let reply = JSON.parse(x);

      if (reply.success) {
        res.json({
          success: true,
        });
      } else {
        res.json({
          success: false,
          msg: 'Something went wrong'
        });
      }

    })
  } catch (e) {
    console.log(e);
    res.json({
      success: false,
      msg: 'Something went wrong'
    })
  }
});

module.exports = router;
