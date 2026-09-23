<!-- app/pages/discography.vue -->
<template>
  <div class="min-h-screen bg-[#0a0a0a] text-white pt-32 pb-20 px-6 md:px-12 max-w-7xl mx-auto">
    <p class="text-xs uppercase tracking-widest text-[#888] mb-2">DISCOGRAPHY</p>
    <h1 class="text-5xl md:text-6xl font-serif italic mb-12">Complete Works</h1>

    <p v-if="store.loading" class="text-[#888] text-sm">Loading discography...</p>
    <p v-else-if="!store.items.length" class="text-[#888] text-sm">No discography entries yet.</p>

    <!-- Timeline / List Standard -->
    <div v-else class="space-y-12">
      <div v-for="yearGroup in store.groupedByYear" :key="yearGroup.year" class="grid grid-cols-12 gap-4 items-start border-t border-[#222] pt-6">
        <!-- Year Column -->
        <div class="col-span-12 md:col-span-2">
          <span class="text-3xl font-serif italic text-[#666]">{{ yearGroup.year }}</span>
        </div>

        <!-- Releases List Column -->
        <div class="col-span-12 md:col-span-10 divide-y divide-[#1a1a1a]">
          <a
            v-for="item in yearGroup.entries"
            :key="item.id"
            :href="item.externalUrl || undefined"
            target="_blank"
            class="py-4 first:pt-0 flex items-center justify-between group cursor-pointer hover:bg-[#121212] px-2 transition"
          >
            <div class="flex items-center gap-4">
              <span class="text-xs uppercase px-2 py-0.5 bg-[#1a1a1a] text-[#888] border border-[#222]">{{ item.role }}</span>
              <span class="text-lg font-serif italic text-white group-hover:text-[#ccff00] transition">{{ item.title }}</span>
            </div>
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useDiscographyStore } from '~/stores/discography.store';

const store = useDiscographyStore();

onMounted(() => {
  store.fetchDiscography();
});
</script>