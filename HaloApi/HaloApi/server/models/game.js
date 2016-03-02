var mongoose = require('mongoose');

module.export = mongoose.model('Game', {
    name: { type: String, required: true }
});