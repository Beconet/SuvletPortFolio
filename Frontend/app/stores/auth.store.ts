// app/stores/auth.store.ts
import { defineStore } from 'pinia';

interface LoginResponse {
  token: string;
  expiresAt: string;
}

export const useAuthStore = defineStore('authStore', () => {
  const token = useCookie<string | null>('admin_token', {
    default: () => null,
    maxAge: 60 * 60 * 24 * 2,
    sameSite: 'lax',
  });

  const error = ref<string | null>(null);
  const loading = ref(false);

  const isAuthenticated = computed(() => !!token.value);

  const login = async (password: string) => {
    const config = useRuntimeConfig();
    error.value = null;
    loading.value = true;
    try {
      const res = await $fetch<LoginResponse>(`${config.public.apiBase}/api/auth/login`, {
        method: 'POST',
        body: { password },
      });
      token.value = res.token;
    } catch (err: any) {
      error.value = err?.data?.message || 'Invalid password.';
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const logout = () => {
    token.value = null;
  };

  return {
    token,
    error,
    loading,
    isAuthenticated,
    login,
    logout,
  };
});
