const amqp = require('amqplib/callback_api');
const config = require('../config/config')

// Create a new survey
module.exports.createNewSurvey = function(survey) {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, function(err, conn) {
      conn.createChannel(function(err, ch) {
        ch.assertQueue('', {
          exclusive: true
        }, function(err, q) {
          const corr = generateUuid();

          ch.consume(q.queue, function(msg) {
            if (msg.properties.correlationId === corr) {
              //console.log(' [.] Got %s', msg.content.toString());
              resolve(msg.content.toString());
              setTimeout(function() {
                conn.close();
              }, 500);
            }
          }, {
            noAck: true
          });

          ch.sendToQueue('rpc_save_survey',
            new Buffer(JSON.stringify(survey)), {
              correlationId: corr,
              replyTo: q.queue
            });
        });
      });
    });
  })
}

// Get survey overview
module.exports.getSurvey = function(name) {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, function(err, conn) {
      conn.createChannel(function(err, ch) {
        ch.assertQueue('', {
          exclusive: true
        }, function(err, q) {
          let corr = generateUuid();
          console.log('sending a get survey request..');

          ch.consume(q.queue, function(msg) {
            if (msg.properties.correlationId === corr) {
              //console.log(' [.] Got %s', msg.content.toString());
              resolve(msg.content.toString());
              setTimeout(function() {
                conn.close();
              }, 500);
            }
          }, {
            noAck: true
          });

          ch.sendToQueue('rpc_return_surveys_unpop',
            new Buffer(name), {
              correlationId: corr,
              replyTo: q.queue
            });
        });
      });
    });
  })
}

// Get specific survey data
module.exports.getSurveyDataByID = (surveyID) => {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, function(err, conn) {
      conn.createChannel(function(err, ch) {
        ch.assertQueue('', {
          exclusive: true
        }, function(err, q) {
          let corr = generateUuid();

          ch.consume(q.queue, function(msg) {
            if (msg.properties.correlationId === corr) {
              resolve(msg.content.toString());
              setTimeout(function() {
                conn.close();
              }, 500);
            }
          }, {
            noAck: true
          });

          ch.sendToQueue('rpc_single_survey',
            new Buffer(surveyID), {
              correlationId: corr,
              replyTo: q.queue
            });
        });
      });
    });
  })
}

module.exports.deleteSurvey = () => {
  // Delete specific survey
  // todo
}

// Get a specific survey
module.exports.getSurveyByID = () => {
  // todo
}

// Send in answers to a specific survey
module.exports.sendAnswers = () => {
  // todo
}

// Generate ID
function generateUuid() {
  return Math.random().toString() +
    Math.random().toString() +
    Math.random().toString();
}
