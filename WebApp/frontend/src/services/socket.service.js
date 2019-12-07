import axios from "axios"

export default {
    getAll() {
        return axios.get("/api/enemies").then(res => res.data)
    },
    connect(ip, port) {
        return axios.get("/api/connect/" + ip + "/" + port).then(res => res.data)
    },
    action(name, id) {
        return axios.get("/api/action/" + name + "/" + id).then(res => res.data)
    }
}
