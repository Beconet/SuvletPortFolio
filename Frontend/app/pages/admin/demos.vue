<!-- app/pages/admin/demos.vue -->
<template>
  <div>
    <p class="text-xs uppercase tracking-widest text-[#888] mb-2">DEMO TRACKS</p>
    <h1 class="text-4xl font-serif italic mb-8">Manage Demo Tracks</h1>

    <div class="grid grid-cols-1 lg:grid-cols-12 gap-8">
      <!-- List -->
      <div class="lg:col-span-7">
        <p v-if="store.loading" class="text-[#888] text-sm">Loading tracks...</p>
        <p v-else-if="!store.tracks.length" class="text-[#888] text-sm">No demo tracks yet.</p>

        <div v-else class="border border-[#222] divide-y divide-[#222]">
          <div v-for="track in store.tracks" :key="track.id" class="p-4 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
            <div class="min-w-0">
              <p class="font-serif italic text-lg truncate">{{ track.name }}</p>
              <p class="text-xs text-[#888]">{{ track.genre }} · {{ track.description }}</p>
            </div>
            <div class="flex items-center gap-3 shrink-0">
              <button @click="editTrack(track)" class="text-xs text-[#888] hover:text-[#ccff00] transition">Edit</button>
              <button @click="handleDelete(track.id)" class="text-xs text-[#888] hover:text-red-400 transition">Delete</button>
            </div>
          </div>
        </div>
      </div>

      <!-- Form -->
      <div class="lg:col-span-5">
        <div class="border border-[#222] bg-[#121212] p-6 flex flex-col gap-4">
          <p class="text-xs uppercase tracking-widest text-[#888]">{{ editingId ? 'Edit Track' : 'Add Track' }}</p>

          <div>
            <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Name</label>
            <input v-model="form.name" required class="input" />
          </div>
          <div>
            <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Genre</label>
            <input v-model="form.genre" required class="input" placeholder="e.g. Ambient" />
          </div>
          <div>
            <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Description</label>
            <textarea v-model="form.description" rows="2" class="input"></textarea>
          </div>
          <div>
            <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Audio</label>
            <div class="flex flex-col sm:flex-row gap-3">
              <input v-model="form.audioUrl" placeholder="https://..." class="input flex-1" />
              <label class="border border-[#333] hover:border-[#666] px-4 py-3 text-xs cursor-pointer transition text-center whitespace-nowrap">
                {{ uploading ? 'Uploading...' : 'Upload' }}
                <input type="file" accept="audio/*" class="hidden" @change="onFileChange" />
              </label>
            </div>
          </div>

          <p v-if="error" class="text-xs text-red-400">{{ error }}</p>

          <div class="flex gap-3 mt-2">
            <button
              @click="handleSubmit"
              :disabled="submitting"
              class="bg-[#ccff00] text-[#0a0a0a] px-5 py-3 text-sm font-medium hover:bg-[#b8e600] transition disabled:opacity-50"
            >
              {{ submitting ? 'Saving...' : editingId ? 'Update Track' : 'Add Track' }}
            </button>
            <button v-if="editingId" @click="resetForm" class="border border-[#333] hover:border-[#666] px-5 py-3 text-sm font-medium transition">
              Cancel
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useDemoTrackStore } from '~/stores/demotracks.store';
import { uploadFile } from '~/services/api';
import type { DemoTrack } from '~/types';

definePageMeta({ layout: 'admin', middleware: 'auth' });

const store = useDemoTrackStore();

onMounted(() => {
  store.fetchDemoTracks();
});

const editingId = ref<string | null>(null);
const uploading = ref(false);
const submitting = ref(false);
const error = ref('');

const form = reactive({
  name: '',
  genre: '',
  description: '',
  audioUrl: '',
});

const resetForm = () => {
  editingId.value = null;
  form.name = '';
  form.genre = '';
  form.description = '';
  form.audioUrl = '';
  error.value = '';
};

const editTrack = (track: DemoTrack) => {
  editingId.value = track.id;
  form.name = track.name;
  form.genre = track.genre;
  form.description = track.description;
  form.audioUrl = track.audioUrl;
};

const onFileChange = async (event: Event) => {
  const input = event.target as HTMLInputElement;
  const file = input.files?.[0];
  if (!file) return;
  uploading.value = true;
  try {
    const result = await uploadFile(file);
    form.audioUrl = result.fileUrl;
  } catch {
    error.value = 'File upload failed.';
  } finally {
    uploading.value = false;
  }
};

const handleSubmit = async () => {
  if (!form.name) {
    error.value = 'Name is required.';
    return;
  }
  error.value = '';
  submitting.value = true;
  try {
    if (editingId.value) {
      await store.updateTrack(editingId.value, { ...form });
    } else {
      await store.createTrack({ ...form });
    }
    resetForm();
  } catch {
    error.value = 'Failed to save demo track.';
  } finally {
    submitting.value = false;
  }
};

const handleDelete = async (id: string) => {
  if (!confirm('Delete this demo track?')) return;
  try {
    await store.deleteTrack(id);
    if (editingId.value === id) resetForm();
  } catch {
    error.value = 'Failed to delete demo track.';
  }
};
</script>

<style scoped>
.input {
  @apply w-full bg-[#0a0a0a] border border-[#222] px-4 py-3 text-sm text-white focus:outline-none focus:border-[#ccff00] transition;
}
</style>
