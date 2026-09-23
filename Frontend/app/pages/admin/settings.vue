<!-- app/pages/admin/settings.vue -->
<template>
  <div class="max-w-2xl">
    <p class="text-xs uppercase tracking-widest text-[#888] mb-2">SETTINGS</p>
    <h1 class="text-4xl font-serif italic mb-10">Profile & Contact</h1>

    <!-- Availability Message -->
    <section class="border border-[#222] bg-[#121212] p-6 mb-8">
      <p class="text-xs uppercase tracking-widest text-[#888] mb-4">Availability Message</p>
      <div class="flex flex-col sm:flex-row gap-3">
        <input v-model="availabilityStatus" class="input flex-1" placeholder="Available for bookings & collaborations" />
        <button @click="saveAvailability" :disabled="savingAvailability" class="bg-[#ccff00] text-[#0a0a0a] px-5 py-3 text-sm font-medium hover:bg-[#b8e600] transition disabled:opacity-50 whitespace-nowrap">
          Save
        </button>
      </div>
      <p v-if="savedAvailability" class="text-xs text-[#ccff00] mt-2">Saved.</p>
    </section>

    <!-- Profile & Contact -->
    <section class="border border-[#222] bg-[#121212] p-6 mb-8 flex flex-col gap-4">
      <p class="text-xs uppercase tracking-widest text-[#888]">Profile & Contact</p>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Description</label>
        <textarea v-model="profileForm.description" rows="4" class="input"></textarea>
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Email</label>
        <input v-model="profileForm.email" class="input" />
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Instagram URL</label>
        <input v-model="profileForm.instagramUrl" class="input" />
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Spotify URL</label>
        <input v-model="profileForm.spotifyUrl" class="input" />
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">SoundCloud URL</label>
        <input v-model="profileForm.soundcloudUrl" class="input" />
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">YouTube URL</label>
        <input v-model="profileForm.youtubeUrl" class="input" />
      </div>

      <p v-if="profileError" class="text-xs text-red-400">{{ profileError }}</p>
      <button @click="saveProfile" :disabled="savingProfile" class="bg-[#ccff00] text-[#0a0a0a] px-5 py-3 text-sm font-medium hover:bg-[#b8e600] transition disabled:opacity-50 self-start">
        {{ savingProfile ? 'Saving...' : 'Save Profile' }}
      </button>
      <p v-if="savedProfile" class="text-xs text-[#ccff00]">Saved.</p>
    </section>
  </div>
</template>

<script setup lang="ts">
import { useSettingsStore } from '~/stores/settings.store';
import { useProfileStore } from '~/stores/profile.store';

definePageMeta({ layout: 'admin', middleware: 'auth' });

const settingsStore = useSettingsStore();
const profileStore = useProfileStore();

const availabilityStatus = ref('');
const savingAvailability = ref(false);
const savedAvailability = ref(false);

const profileForm = reactive({
  description: '',
  email: '',
  instagramUrl: '',
  spotifyUrl: '',
  soundcloudUrl: '',
  youtubeUrl: '',
});
const savingProfile = ref(false);
const savedProfile = ref(false);
const profileError = ref('');

onMounted(async () => {
  await Promise.all([settingsStore.fetchSettings(), profileStore.fetchProfile()]);

  availabilityStatus.value = settingsStore.getValue('AvailabilityStatus');

  if (profileStore.profile) {
    profileForm.description = profileStore.profile.description;
    profileForm.email = profileStore.profile.email;
    profileForm.instagramUrl = profileStore.profile.instagramUrl || '';
    profileForm.spotifyUrl = profileStore.profile.spotifyUrl || '';
    profileForm.soundcloudUrl = profileStore.profile.soundcloudUrl || '';
    profileForm.youtubeUrl = profileStore.profile.youtubeUrl || '';
  }
});

const saveAvailability = async () => {
  savingAvailability.value = true;
  savedAvailability.value = false;
  try {
    await settingsStore.upsertSetting('AvailabilityStatus', availabilityStatus.value);
    savedAvailability.value = true;
  } finally {
    savingAvailability.value = false;
  }
};

const saveProfile = async () => {
  profileError.value = '';
  savingProfile.value = true;
  savedProfile.value = false;
  try {
    await profileStore.updateProfile({ ...profileForm });
    savedProfile.value = true;
  } catch {
    profileError.value = 'Failed to save profile.';
  } finally {
    savingProfile.value = false;
  }
};
</script>

<style scoped>
.input {
  @apply w-full bg-[#0a0a0a] border border-[#222] px-4 py-3 text-sm text-white focus:outline-none focus:border-[#ccff00] transition;
}
</style>
