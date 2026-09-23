// app/middleware/auth.ts
import { useAuthStore } from '~/stores/auth.store';

export default defineNuxtRouteMiddleware((to) => {
  const authStore = useAuthStore();

  if (!authStore.isAuthenticated && to.path !== '/admin/login') {
    return navigateTo('/admin/login');
  }
});
