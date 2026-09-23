<!-- app/pages/admin/releases/index.vue -->
<template>
  <div>
    <div class="flex flex-col sm:flex-row sm:items-end justify-between gap-4 mb-8">
      <div>
        <p class="text-xs uppercase tracking-widest text-[#888] mb-2">RELEASES</p>
        <h1 class="text-4xl font-serif italic">Manage Releases</h1>
      </div>
      <NuxtLink to="/admin/releases/new" class="bg-[#ccff00] text-[#0a0a0a] px-5 py-3 text-sm font-medium hover:bg-[#b8e600] transition self-start">
        + New Release
      </NuxtLink>
    </div>

    <input
      v-model="search"
      type="text"
      placeholder="Search by title..."
      class="w-full max-w-sm bg-[#121212] border border-[#222] px-4 py-2.5 text-sm text-white mb-6 focus:outline-none focus:border-[#ccff00] transition"
    />

    <p v-if="store.loading" class="text-[#888] text-sm">Loading releases...</p>
    <p v-else-if="!filtered.length" class="text-[#888] text-sm">No releases found.</p>

    <div v-else class="border border-[#222] divide-y divide-[#222]">
      <div v-for="release in filtered" :key="release.id" class="p-4 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
        <div class="min-w-0">
          <p class="font-serif italic text-lg truncate">{{ release.title }}</p>
          <p class="text-xs text-[#888]">{{ release.formats }} · {{ new Date(release.releaseDate).getFullYear() }}</p>
        </div>
        <NuxtLink :to="`/admin/releases/${release.id}`" class="text-xs text-[#888] hover:text-[#ccff00] transition shrink-0">
          Edit ↗
        </NuxtLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useReleaseStore } from '~/stores/releases.store';

definePageMeta({ layout: 'admin', middleware: 'auth' });

const store = useReleaseStore();
const search = ref('');

onMounted(() => {
  store.fetchReleases();
});

const filtered = computed(() =>
  store.releases.filter(r => r.title.toLowerCase().includes(search.value.toLowerCase()))
);
</script>
