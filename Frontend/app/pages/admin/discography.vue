<!-- app/pages/admin/discography.vue -->
<template>
  <div>
    <p class="text-xs uppercase tracking-widest text-[#888] mb-2">DISCOGRAPHY</p>
    <h1 class="text-4xl font-serif italic mb-8">Manage Discography</h1>

    <div class="grid grid-cols-1 lg:grid-cols-12 gap-8">
      <!-- List -->
      <div class="lg:col-span-7">
        <p v-if="store.loading" class="text-[#888] text-sm">Loading discography...</p>
        <p v-else-if="!store.items.length" class="text-[#888] text-sm">No discography entries yet.</p>

        <div v-else class="border border-[#222] divide-y divide-[#222]">
          <div v-for="item in store.items" :key="item.id" class="p-4 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
            <div class="min-w-0">
              <p class="font-serif italic text-lg truncate">{{ item.title }}</p>
              <p class="text-xs text-[#888]">{{ item.role }} · {{ new Date(item.releaseDate).getFullYear() }}</p>
            </div>
            <div class="flex items-center gap-3 shrink-0">
              <button @click="editItem(item)" class="text-xs text-[#888] hover:text-[#ccff00] transition">Edit</button>
              <button @click="handleDelete(item.id)" class="text-xs text-[#888] hover:text-red-400 transition">Delete</button>
            </div>
          </div>
        </div>
      </div>

      <!-- Form -->
      <div class="lg:col-span-5">
        <div class="border border-[#222] bg-[#121212] p-6 flex flex-col gap-4">
          <p class="text-xs uppercase tracking-widest text-[#888]">{{ editingId ? 'Edit Entry' : 'Add Entry' }}</p>

          <div>
            <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Title</label>
            <input v-model="form.title" required class="input" />
          </div>
          <div>
            <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Role (e.g. Producer, Co-Writer)</label>
            <input v-model="form.role" required class="input" />
          </div>
          <div>
            <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Release Date</label>
            <input v-model="form.releaseDate" type="date" required class="input" />
          </div>
          <div>
            <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">External URL</label>
            <input v-model="form.externalUrl" placeholder="https://..." class="input" />
          </div>

          <p v-if="error" class="text-xs text-red-400">{{ error }}</p>

          <div class="flex gap-3 mt-2">
            <button
              @click="handleSubmit"
              :disabled="submitting"
              class="bg-[#ccff00] text-[#0a0a0a] px-5 py-3 text-sm font-medium hover:bg-[#b8e600] transition disabled:opacity-50"
            >
              {{ submitting ? 'Saving...' : editingId ? 'Update Entry' : 'Add Entry' }}
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
import { useDiscographyStore } from '~/stores/discography.store';
import type { DiscographyItem } from '~/types';

definePageMeta({ layout: 'admin', middleware: 'auth' });

const store = useDiscographyStore();

onMounted(() => {
  store.fetchDiscography();
});

const editingId = ref<string | null>(null);
const submitting = ref(false);
const error = ref('');

const form = reactive({
  title: '',
  role: '',
  releaseDate: '',
  externalUrl: '',
});

const resetForm = () => {
  editingId.value = null;
  form.title = '';
  form.role = '';
  form.releaseDate = '';
  form.externalUrl = '';
  error.value = '';
};

const editItem = (item: DiscographyItem) => {
  editingId.value = item.id;
  form.title = item.title;
  form.role = item.role;
  form.releaseDate = item.releaseDate.slice(0, 10);
  form.externalUrl = item.externalUrl;
};

const handleSubmit = async () => {
  if (!form.title || !form.role || !form.releaseDate) {
    error.value = 'Title, role, and release date are required.';
    return;
  }
  error.value = '';
  submitting.value = true;
  try {
    const payload = {
      title: form.title,
      role: form.role,
      externalUrl: form.externalUrl,
      releaseDate: new Date(form.releaseDate).toISOString(),
    };
    if (editingId.value) {
      await store.updateItem(editingId.value, payload);
    } else {
      await store.createItem(payload);
    }
    resetForm();
  } catch {
    error.value = 'Failed to save discography entry.';
  } finally {
    submitting.value = false;
  }
};

const handleDelete = async (id: string) => {
  if (!confirm('Delete this discography entry?')) return;
  try {
    await store.deleteItem(id);
    if (editingId.value === id) resetForm();
  } catch {
    error.value = 'Failed to delete discography entry.';
  }
};
</script>

<style scoped>
.input {
  @apply w-full bg-[#0a0a0a] border border-[#222] px-4 py-3 text-sm text-white focus:outline-none focus:border-[#ccff00] transition;
}
</style>
