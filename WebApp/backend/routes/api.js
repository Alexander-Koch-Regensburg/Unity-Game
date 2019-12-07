const express = require("express")
const router = express.Router()
const socketService = require("../services/socket.service")

router.get("/connect/:ip/:port", socketService.connect.bind(socketService))
router.get("/action/:name/:id", socketService.action.bind(socketService))
router.get("/enemies", socketService.getAll.bind(socketService))

module.exports = router
