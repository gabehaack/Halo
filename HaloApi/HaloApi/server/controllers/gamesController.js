var Game = require('../models/game')

module.exports.create = function (req, res) {
    var game = new Game(req.body);
    game.save(function(err, result) {
        res.json(result);
    });
}

module.exports.list = function(req, res) {
    Game.find({}, function(err, results) {
        res.json(results);
    });
}