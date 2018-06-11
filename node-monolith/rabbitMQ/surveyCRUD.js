const amqp = require('amqplib/callback_api');
const config = require('../config/config')
// import config with amqp conn

// Create a new survey
module.exports.createNewSurvey = function(survey) {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, function(err, conn) {
      conn.createChannel(function(err, ch) {
        ch.assertQueue('', {
          exclusive: true
        }, function(err, q) {
          let corr = generateUuid();
          let currsurvey = survey;
          console.log('Sending a create survey request..');

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

          ch.sendToQueue('rpc_queue',
            new Buffer(JSON.stringify(currsurvey)), {
              correlationId: corr,
              replyTo: q.queue
            });
        });
      });
    });
  })
}

// Get survey(s)
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

          ch.sendToQueue('rpc_surveys',
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
module.exports.getSurveyData = () => {
  // todo
}

// Delete specific survey
module.exports.deleteSurvey = () => {
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


function generateUuid() {
  return Math.random().toString() +
    Math.random().toString() +
    Math.random().toString();
}







// TEMPLATE 
