import type { NavigationGuardNext, RouteLocationNormalized } from 'vue-router'
import { createRouter, createWebHistory } from 'vue-router'

import MainPage from '@/components/MainPage.vue'
import MainForm from '@/components/MainForm.vue'
import NotFound from '@/components/NotFound.vue'

import { forms } from './store/forms'


const routes = [
  { path: '/', name: 'mainPage', component: MainPage},
  { path: '/Quest', name: 'createForm', component: MainForm },
  { path: '/Quest/:id', name: 'changeForm', component: MainForm, props: true,

    beforeEnter: (to: RouteLocationNormalized, _: RouteLocationNormalized, next: NavigationGuardNext) => {

      if (forms.value.some(f => f.id === to.params.id)) { next(); }
      else { next({ name: 'notFound' }); }
    },
  },
  { path: '/:pathMatch(.*)*', name: 'notFound', component: NotFound },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
