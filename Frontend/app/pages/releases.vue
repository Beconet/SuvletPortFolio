<!-- app/pages/releases.vue -->
<template>
  <div class="min-h-screen bg-[#0a0a0a] text-white pt-32 pb-20 px-6 md:px-12 max-w-7xl mx-auto">
    <!-- Header & Filters -->
    <div class="flex flex-col md:flex-row md:items-end justify-between mb-12 gap-6">
      <div>
        <p class="text-xs uppercase tracking-widest text-[#888] mb-2">RELEASES</p>
        <h1 class="text-5xl md:text-6xl font-serif italic">{{ store.releases.length }} Releases</h1>
      </div>

      <!-- Filter Buttons -->
      <div class="flex items-center gap-2">
        <button 
          v-for="type in ['All', 'Album', 'EP', 'Single']" 
          :key="type"
          @click="store.activeFilter = type"
          :class="[
            'px-4 py-2 text-xs font-medium border transition',
            store.activeFilter === type 
              ? 'border-[#ccff00] text-[#ccff00] bg-[#ccff00]/5' 
              : 'border-[#222] text-[#888] hover:text-white hover:border-[#444]'
          ]"
        >
          {{ type }}
        </button>
      </div>
    </div>

    <!-- Loading State -->
    <p v-if="store.loading" class="text-[#888] text-sm">Loading releases...</p>

    <!-- Empty State -->
    <p v-else-if="!store.filteredReleases.length" class="text-[#888] text-sm">No releases found.</p>

    <!-- Cards Grid -->
    <div v-else class="grid grid-cols-1 md:grid-cols-3 gap-8">
      <ReleaseCard 
        v-for="release in store.filteredReleases" 
        :key="release.id" 
        :release="release" 
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { useReleaseStore } from '~/stores/releases.store';
import ReleaseCard from '~/components/releases/ReleaseCard.vue';

const store = useReleaseStore();

onMounted(() => {
  store.fetchReleases();
});
</script>