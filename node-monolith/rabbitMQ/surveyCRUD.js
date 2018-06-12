"use strict";

const amqp = require('amqplib/callback_api');
const config = require('../config/config')

// Generate ID
function generateUuid() {
  return Math.random().toString() +
    Math.random().toString() +
    Math.random().toString();
};

// RPC Request
module.exports.RPC = (parameter, queue) => {
  return new Promise((resolve, reject) => {
    amqp.connect(config.rabbitConnUrl, (err, conn) => {
      conn.createChannel((err, ch) => {
        ch.assertQueue('', {
          exclusive: true
        }, (err, q) => {

          if(err) {
            reject(err); 
          }

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
