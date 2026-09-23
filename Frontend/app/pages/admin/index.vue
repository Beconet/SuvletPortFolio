<!-- app/pages/admin/index.vue -->
<template>
  <div>
    <p class="text-xs uppercase tracking-widest text-[#888] mb-2">DASHBOARD</p>
    <h1 class="text-4xl font-serif italic mb-10">Overview</h1>

    <div class="grid grid-cols-1 sm:grid-cols-3 gap-6 mb-12">
      <div class="border border-[#222] bg-[#121212] p-6">
        <p class="text-xs uppercase tracking-wider text-[#888] mb-2">Total Releases</p>
        <p class="text-4xl font-serif italic">{{ releaseStore.releases.length }}</p>
      </div>
      <div class="border border-[#222] bg-[#121212] p-6">
        <p class="text-xs uppercase tracking-wider text-[#888] mb-2">Demo Tracks</p>
        <p class="text-4xl font-serif italic">{{ demoStore.tracks.length }}</p>
      </div>
      <div class="border border-[#222] bg-[#121212] p-6">
        <p class="text-xs uppercase tracking-wider text-[#888] mb-2">Availability</p>
        <p class="text-lg font-medium text-[#ccff00]">{{ availabilityStatus || 'Not set' }}</p>
      </div>
    </div>

    <p class="text-xs uppercase tracking-widest text-[#888] mb-4">QUICK ACTIONS</p>
    <div class="flex flex-wrap gap-4">
      <NuxtLink to="/admin/releases/new" class="bg-[#ccff00] text-[#0a0a0a] px-5 py-3 text-sm font-medium hover:bg-[#b8e600] transition">
        + New Release
      </NuxtLink>
      <NuxtLink to="/admin/demos" class="border border-[#333] hover:border-[#666] px-5 py-3 text-sm font-medium transition">
        + New Demo Track
      </NuxtLink>
      <NuxtLink to="/admin/settings" class="border border-[#333] hover:border-[#666] px-5 py-3 text-sm font-medium transition">
        Update Availability
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useReleaseStore } from '~/stores/releases.store';
import { useDemoTrackStore } from '~/stores/demotracks.store';
import { useSettingsStore } from '~/stores/settings.store';

definePageMeta({ layout: 'admin', middleware: 'auth' });

const releaseStore = useReleaseStore();
const demoStore = useDemoTrackStore();
const settingsStore = useSettingsStore();

onMounted(() => {
  releaseStore.fetchReleases();
  demoStore.fetchDemoTracks();
  settingsStore.fetchSettings();
});

const availabilityStatus = computed(() => settingsStore.getValue('AvailabilityStatus'));
</script>
