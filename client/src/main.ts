import { VueQueryPlugin } from '@tanstack/vue-query'
import { createApp } from 'vue'
import AppLayout from '@/layouts/AppLayout.vue'
import router from '@/router/index'
import App from './App.vue'
import './style.css'

const app = createApp(App)
app.component('AppLayout', AppLayout)
app.use(VueQueryPlugin)
app.use(router)
app.mount('#app')
