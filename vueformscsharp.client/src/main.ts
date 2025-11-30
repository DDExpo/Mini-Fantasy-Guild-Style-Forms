import { createApp } from 'vue'

import App from './App.vue'
import router from './router';
import VueVirtualScroller from "vue-virtual-scroller";

import './assets/main.css'
import 'vue-virtual-scroller/dist/vue-virtual-scroller.css'

const app = createApp(App);
app.use(VueVirtualScroller);
app.use(router);

app.mount("#app");

