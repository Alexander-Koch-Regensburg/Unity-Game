import Vue from 'vue'
import App from './App.vue'

Vue.config.productionTip = false

axios.defaults.baseURL = 'http://localhost:1337'
import axios from 'axios'

new Vue({
  render: h => h(App)
}).$mount('#app')
