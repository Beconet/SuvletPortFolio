<!-- app/components/releases/ReleaseCard.vue -->
<template>
  <div class="group cursor-pointer flex flex-col justify-between">
    <!-- Image / Placeholder Box -->
    <div class="aspect-square w-full bg-[#121212] border border-[#222] relative p-6 flex flex-col justify-end overflow-hidden mb-4">
      <img 
        v-if="release.coverImageUrl" 
        :src="release.coverImageUrl" 
        :alt="release.title" 
        class="absolute inset-0 w-full h-full object-cover group-hover:scale-105 transition-transform duration-500 opacity-80"
      />
      <!-- Abstract Gradient Background หากไม่มีรูป -->
      <div v-else class="absolute inset-0 bg-gradient-to-br from-purple-900/40 via-black to-black group-hover:scale-105 transition-transform duration-500"></div>

      <!-- Tag inside Art -->
      <span class="relative z-10 text-[10px] uppercase tracking-widest text-white/80 font-medium">
        {{ release.formats }} · {{ releaseYear }}
      </span>
    </div>

    <!-- Metadata -->
    <div>
      <div class="flex items-start justify-between mb-1">
        <h3 class="text-2xl font-serif italic text-white group-hover:text-[#ccff00] transition">
          {{ release.title }}
        </h3>
      </div>
      <p class="text-xs text-[#888] mb-2">{{ release.formats }} · {{ releaseYear }}</p>
      <p v-if="release.description" class="text-sm text-white/60 line-clamp-2 font-light mb-4">
        {{ release.description }}
      </p>

      <a 
        v-if="release.spotifyUrl" 
        :href="release.spotifyUrl" 
        target="_blank" 
        class="text-xs text-white/80 hover:text-[#ccff00] transition flex items-center gap-1"
      >
        Spotify ↗
      </a>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Release } from '~/types';

const props = defineProps<{
  release: Release
}>();

const releaseYear = computed(() => new Date(props.release.releaseDate).getFullYear());
</script>