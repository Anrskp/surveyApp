const amqp = require('amqplib/callback_api');
const config = require('../config/config')

/*
// Create a new survey
module.exports.createNewSurvey = (survey) => {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, (err, conn) => {
      conn.createChannel((err, ch) => {
        ch.assertQueue('', {
          exclusive: true
        }, (err, q) => {
          const corr = generateUuid();

          ch.consume(q.queue, (msg) => {
            if (msg.properties.correlationId === corr) {
              resolve(msg.content.toString());
              setTimeout(() => {
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
module.exports.getSurvey = (name) => {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, (err, conn) => {
      conn.createChannel((err, ch) => {
        ch.assertQueue('', {
          exclusive: true
        }, (err, q) => {
          let corr = generateUuid();
          console.log('sending a get survey request..');

          ch.consume(q.queue, (msg) => {
            if (msg.properties.correlationId === corr) {
              //console.log(' [.] Got %s', msg.content.toString());
              resolve(msg.content.toString());
              setTimeout(() => {
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

// Get specific survey
module.exports.getSurveyByID = (surveyID) => {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, (err, conn) => {
      conn.createChannel((err, ch) => {
        ch.assertQueue('', {
          exclusive: true
        }, (err, q) => {
          let corr = generateUuid();

          ch.consume(q.queue, (msg) => {
            if (msg.properties.correlationId === corr) {
              resolve(msg.content.toString());
              setTimeout(() => {
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

module.exports.deleteSurvey = (surveyID) => {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, (err, conn) => {
      conn.createChannel((err, ch) => {
        ch.assertQueue('', {
          exclusive: true
        }, (err, q) => {
          let corr = generateUuid();

          ch.consume(q.queue, (msg) => {
            if (msg.properties.correlationId === corr) {
              resolve(msg.content.toString());
              setTimeout(() => {
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

// Get a specific surveys results data
module.exports.getSurveyResultsByID = (surveyID) => {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, (err, conn) => {
      conn.createChannel((err, ch) => {
        ch.assertQueue('', {
          exclusive: true
        }, (err, q) => {
          let corr = generateUuid();

          ch.consume(q.queue, (msg) => {
            if (msg.properties.correlationId === corr) {
              resolve(msg.content.toString());
              setTimeout(() => {
                conn.close()
              }, 500)
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

// Send in answers to a specific survey
module.exports.sendAnswers = (surveyID) => {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, (err, conn) => {
      conn.createChannel((err, ch) => {
        ch.assertQueue('', {
          exclusive: true
        }, (err, q) => {
          let corr = generateUuid();

          ch.consume(q.queue, (msg) => {
            if (msg.properties.correlationId === corr) {
              resolve(msg.content.toString());
              setTimeout(() => {
                conn.close()
              }, 500)
            }
          }, {
            noAck: true
          });

          ch.sendToQueue('get_answers', new Buffer(surveyID), {
            correlationId: corr,
            replyTo: q.queue
          });
        });
      });
    });
  })
}
*/
// Generate ID
function generateUuid() {
  return Math.random().toString() +
    Math.random().toString() +
    Math.random().toString();
}



// TESTING
module.exports.RPC = (parameter, queue) => {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, (err, conn) => {
      conn.createChannel((err, ch) => {
        ch.assertQueue('', {
          exclusive: true
        }, (err, q) => {
          let corr = generateUuid();

          ch.consume(q.queue, (msg) => {
            if (msg.properties.correlationId === corr) {
              resolve(msg.content.toString());
              setTimeout(() => {
                conn.close()
              }, 500)
            }
          }, {
            noAck: true
          });

          ch.sendToQueue(queue, new Buffer(parameter), {
            correlationId: corr,
            replyTo: q.queue
          });
        });
      });
    });
  })
}
