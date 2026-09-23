<!-- app/layouts/admin.vue -->
<template>
  <div class="min-h-screen flex flex-col md:flex-row">
    <aside class="w-full md:w-56 border-b md:border-b-0 md:border-r border-[#222] p-6 flex flex-col gap-1 shrink-0">
      <div class="flex items-center justify-between md:mb-8">
        <NuxtLink to="/admin" class="text-lg font-serif italic">Suvlet Admin</NuxtLink>
        <button @click="isOpen = !isOpen" class="md:hidden text-white p-2 -mr-2" aria-label="Toggle menu">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" :d="isOpen ? 'M6 18L18 6M6 6l12 12' : 'M4 6h16M4 12h16M4 18h16'" />
          </svg>
        </button>
      </div>
      <nav :class="['flex-col gap-1', isOpen ? 'flex mt-4' : 'hidden', 'md:flex md:mt-0']">
        <NuxtLink @click="isOpen = false" to="/admin" exact-active-class="text-[#ccff00]" class="text-sm text-white/70 hover:text-white py-2 transition">Dashboard</NuxtLink>
        <NuxtLink @click="isOpen = false" to="/admin/releases" active-class="text-[#ccff00]" class="text-sm text-white/70 hover:text-white py-2 transition">Releases</NuxtLink>
        <NuxtLink @click="isOpen = false" to="/admin/discography" active-class="text-[#ccff00]" class="text-sm text-white/70 hover:text-white py-2 transition">Discography</NuxtLink>
        <NuxtLink @click="isOpen = false" to="/admin/demos" active-class="text-[#ccff00]" class="text-sm text-white/70 hover:text-white py-2 transition">Demo Tracks</NuxtLink>
        <NuxtLink @click="isOpen = false" to="/admin/settings" active-class="text-[#ccff00]" class="text-sm text-white/70 hover:text-white py-2 transition">Settings</NuxtLink>
        <button @click="handleLogout" class="md:mt-8 text-xs text-[#888] hover:text-[#ccff00] text-left py-2 transition">Log out</button>
      </nav>
    </aside>
    <main class="flex-1 p-6 md:p-12 overflow-y-auto">
      <slot />
    </main>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '~/stores/auth.store';

const authStore = useAuthStore();
const isOpen = ref(false);

const handleLogout = () => {
  authStore.logout();
  navigateTo('/admin/login');
};
</script>
