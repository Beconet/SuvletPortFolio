<!-- app/pages/admin/login.vue -->
<template>
  <div class="min-h-screen bg-[#0a0a0a] text-white flex items-center justify-center px-6">
    <div class="w-full max-w-sm border border-[#222] bg-[#121212] p-8">
      <p class="text-xs uppercase tracking-widest text-[#888] mb-2">ADMIN</p>
      <h1 class="text-3xl font-serif italic mb-8">Sign in</h1>

      <form @submit.prevent="handleSubmit" class="flex flex-col gap-4">
        <div>
          <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Password</label>
          <input
            v-model="password"
            type="password"
            required
            class="w-full bg-[#0a0a0a] border border-[#222] px-4 py-3 text-sm text-white focus:outline-none focus:border-[#ccff00] transition"
          />
        </div>

        <p v-if="authStore.error" class="text-xs text-red-400">{{ authStore.error }}</p>

        <button
          type="submit"
          :disabled="authStore.loading"
          class="bg-[#ccff00] text-[#0a0a0a] px-6 py-3 font-medium text-sm hover:bg-[#b8e600] transition disabled:opacity-50"
        >
          {{ authStore.loading ? 'Signing in...' : 'Sign in' }}
        </button>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '~/stores/auth.store';

definePageMeta({ layout: false });

const authStore = useAuthStore();
const password = ref('');

onMounted(() => {
  if (authStore.isAuthenticated) navigateTo('/admin');
});

const handleSubmit = async () => {
  try {
    await authStore.login(password.value);
    navigateTo('/admin');
  } catch {
    // error message is set on the store
  }
};
</script>
