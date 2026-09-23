<!-- app/components/common/Navbar.vue -->
<template>
  <header class="fixed top-0 left-0 w-full z-50 bg-[#0a0a0a]/80 backdrop-blur-md border-b border-[#222]">
    <div class="max-w-7xl mx-auto px-6 h-20 flex items-center justify-between">
      <!-- Brand Logo & Availability Indicator -->
      <div class="flex items-center gap-4">
        <NuxtLink to="/" class="text-xl font-bold tracking-widest text-white uppercase">SUVLET</NuxtLink>
        <div v-if="availabilityStatus" class="hidden sm:flex items-center gap-2 text-xs text-[#888] border border-[#222] px-2.5 py-1 rounded-full">
          <span class="w-2 h-2 rounded-full bg-[#ccff00] animate-pulse"></span>
          {{ availabilityStatus }}
        </div>
      </div>

      <!-- Desktop Nav -->
      <nav class="hidden md:flex items-center gap-8 text-sm font-medium">
        <NuxtLink to="/" active-class="text-[#ccff00]" class="text-white/70 hover:text-white transition">Home</NuxtLink>
        <NuxtLink to="/profile" active-class="text-[#ccff00]" class="text-white/70 hover:text-white transition">Profile</NuxtLink>
        <NuxtLink to="/releases" active-class="text-[#ccff00]" class="text-white/70 hover:text-white transition">Releases</NuxtLink>
        <NuxtLink to="/discography" active-class="text-[#ccff00]" class="text-white/70 hover:text-white transition">Discography</NuxtLink>
        <NuxtLink to="/contact" active-class="text-[#ccff00]" class="text-white/70 hover:text-white transition">Contact</NuxtLink>
        <NuxtLink to="/listen" active-class="text-[#ccff00]" class="text-white/70 hover:text-white transition">Listen</NuxtLink>
      </nav>

      <!-- Mobile Menu Button -->
      <button @click="isOpen = !isOpen" class="md:hidden text-white focus:outline-none p-2">
        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" :d="isOpen ? 'M6 18L18 6M6 6l12 12' : 'M4 6h16M4 12h16M4 18h16'" />
        </svg>
      </button>
    </div>

    <!-- Mobile Dropdown Menu -->
    <div v-if="isOpen" class="md:hidden bg-[#0a0a0a] border-b border-[#222] px-6 py-6 flex flex-col gap-4 text-base">
      <NuxtLink @click="isOpen = false" to="/" class="text-white/80 hover:text-[#ccff00]">Home</NuxtLink>
      <NuxtLink @click="isOpen = false" to="/profile" class="text-white/80 hover:text-[#ccff00]">Profile</NuxtLink>
      <NuxtLink @click="isOpen = false" to="/releases" class="text-white/80 hover:text-[#ccff00]">Releases</NuxtLink>
      <NuxtLink @click="isOpen = false" to="/discography" class="text-white/80 hover:text-[#ccff00]">Discography</NuxtLink>
      <NuxtLink @click="isOpen = false" to="/contact" class="text-white/80 hover:text-[#ccff00]">Contact</NuxtLink>
      <NuxtLink @click="isOpen = false" to="/listen" class="text-white/80 hover:text-[#ccff00]">Listen</NuxtLink>
    </div>
  </header>
</template>

<script setup lang="ts">
import { fetchApi } from '~/services/api';

const isOpen = ref(false);
const availabilityStatus = ref('');

onMounted(async () => {
  try {
    const setting = await fetchApi<{ value: string }>('/api/settings/AvailabilityStatus');
    availabilityStatus.value = setting.value;
  } catch {
    availabilityStatus.value = '';
  }
});
</script>