<script setup lang="ts">
  import { computed, onMounted } from 'vue';
  import { useRouter } from 'vue-router'

  import { forms, FilteringActionsStore as FilteringAS } from '@/store/forms'
  import { dangerColor } from "@/helpers/uiHelpers";
  import { fetchAllFormsApi, deleteFormApi } from '@/api/forms';

  const router = useRouter()

  onMounted(async () => {
    try {
      const data = await fetchAllFormsApi()

      forms.value = Array.isArray(data)
        ? data.map((item: any) => ({
          id: String(item.id),
          data: item.data ?? {}
        }))
        : [];

    } catch (err) {
      console.error("Failed to find Quests", err)
    }
  })

  function createFormPage(): void { router.push('/Quest') }
  function changeFormPage(id: string): void { router.push(`/Quest/${id}`) }

  async function deleteForm(id: string) {

    const index = forms.value.findIndex(f => f.id === id);
    if (index === -1) return;

    const removedForm = forms.value.splice(index, 1)[0];

    try {
      const data = await deleteFormApi(id);

    } catch (err) {
      forms.value.splice(index, 0, removedForm!);
      alert('Failed to removed Quest: ' + err);
    }
  }

  const filteredForms = computed(() => {
    const query = FilteringAS.searchQuery.toLowerCase();

    const filtered = forms.value.filter((form) => {

      const fields = [
        form.data.questName, form.data.questType, form.data.location,
        form.data.dangerLevel, form.data.rewardGold, form.id
      ];

      return fields.some(field => field?.toString().toLowerCase().includes(query));
    });

    filtered.sort((a, b) => {
      const field = FilteringAS.sortField;

      const aValue = a.data[field];
      const bValue = b.data[field];
      if (typeof aValue === 'string' && typeof bValue === 'string') {
        return FilteringAS.sortOrder == 'asc'
          ? aValue.localeCompare(bValue)
          : bValue.localeCompare(aValue);
      }
      if (typeof aValue === 'number' && typeof bValue === 'number') {
        return FilteringAS.sortOrder === 'asc' ? aValue - bValue : bValue - aValue;
      }
      return 0;
    });
    return filtered;
  });

  function toggleSortOrder(): void { FilteringAS.sortOrder = FilteringAS.sortOrder === 'asc' ? 'desc' : 'asc'; }

</script>

  <template>
    <link href="https://fonts.googleapis.com/css2?family=VT323&display=swap" rel="stylesheet">
    <div class="page">

      <div class="controls">
        <button type="button" class="create" @click="createFormPage">CREATE</button>

        <div class="space"></div>

        <input id="search" type="text" v-model="FilteringAS.searchQuery" placeholder="SEARCH..." />

        <button type="button" class="askDesc" @click="toggleSortOrder">
          {{ FilteringAS.sortOrder === 'asc' ? 'ASC' : 'DESC' }}
        </button>

        <select name="select-sort" v-model="FilteringAS.sortField" aria-label="Choose a sort field">
          <option value="questName">QUEST NAME</option>
          <option value="questType">QUEST TYPE</option>
          <option value="location">LOCATION</option>
          <option value="questDetails">DETAILS</option>
          <option value="dangerLevel">DANGER LEVEL</option>
          <option value="rewardGold">REWARD GOLD</option>
          <option value="deadline">DEADLINE</option>
          <option value="color">COLOR</option>
        </select>

      </div>

      <div class="forms-area">

        <RecycleScroller class="scroller" :items="filteredForms" :item-size="148"
                         key-field="id" v-slot="{ item }">

          <div class="form">

            <div class="form-actions">
              <button type="button" class="change-btn" @click="changeFormPage(item.id)">CHANGE</button>
              <button type="button" class="delete-btn" @click="deleteForm(item.id)">DELETE</button>
            </div>
            <h1 class="form-title">>{{ item.data.questName }}</h1>
            <div class="grid-description">
              <label class="form-quest-type">{{ item.data.questType }}</label>
              <label class="form-danger-level" :style="{ color: dangerColor(item.data.dangerLevel) }">⚔️ {{item.data.dangerLevel}}/10</label>
              <label class="form-location">{{ item.data.location }}</label>
              <label class="form-reward">{{ item.data.rewardGold }} <img src="/coin.svg" alt="gold coin" width="16" height="16" /></label>
            </div>
            <label class="form-stamp" :style="{ background: item.data.color}">A</label>

          </div>
        </RecycleScroller>
      </div>

    </div>
  </template>

  <style scoped>

    * {
      box-sizing: border-box;
      font-family: 'VT323', monospace;
      -webkit-font-smoothing: none;
    }

    .page {
      width: 100%;
      height: 100vh;
      display: flex;
      flex-direction: column;
      align-items: center;
      background-color: transparent;
      -webkit-font-smoothing: none;
    }

    .controls {
      display: flex;
      width: 100%;
      max-width: 700px;
      gap: 8px;
      margin-top: 120px;
      margin-bottom: 20px;
      align-items: center;
      flex-wrap: wrap;
    }

    .space {
      flex-grow: 1;
    }

    .controls button,
    .controls select,
    .controls input {
      min-width: 36px;
      background: #0b0f1ddb;
      color: white;
      padding: 8px 16px;
      border-radius: 0px;
      border: 2px solid rgb(254, 231, 168, 0.9);
      box-shadow: 4px 6px 0px 0px rgba(0, 0, 0, 0.7);
      font-size: 20px;
    }

    .controls input {
      width: 160px;
    }

      .controls input:focus,
      .controls select:focus {
        outline: none;
      }

    .controls button:active {
      transform: translate(2px, 2px);
      box-shadow: 2px 2px 0px 0px #000;
    }

    .controls button:hover,
    .controls input:hover,
    .controls select:hover {
      background: #000000;
    }

    .controls .create:hover {
      background: #232dae;
    }

    .scroller {
      height: 100%;
      width: 100%;
      flex: 1;
      overflow-y: auto;
      padding-bottom: 24px;
    }

    .forms-area {
      position: relative;
      background: #d6b98c;
      box-shadow: 10px 10px 0 rgba(0, 0, 0, 0.7), inset 0 0 0 4px #b47732;
      width: 100%;
      min-height: 100vh;
      max-width: 700px;
      padding: 10px 8px 8px 8px;
      flex-direction: column;
      overflow: hidden;
      border-top: 8px solid #7c4a16;
      border-left: 6px solid #7c4a16;
      border-right: 6px solid #7c4a16;
      display: flex;
      justify-content: center;
    }

    .form {
      position: relative;
      height: 120px;
      background: #f3e6c8;
      border: 3px solid #6a3d12;
      box-shadow: 4px 4px 0 #3e240b, inset 0 0 0 2px #caa46a;
      color: #3a220c;
      image-rendering: pixelated;
      transition: transform 0.12s, box-shadow 0.12s steps(2);
      border-radius: 4px;
      padding: 0 8px;
      margin: 8px;
    }

    .form-title {
      width: 100%;
      max-height: fit-content;
      font-weight: 700;
      font-size: 28px;
      text-overflow: ellipsis;
      overflow: hidden;
      white-space: nowrap;
    }
    .form-stamp {
      position: absolute;
      bottom: -16px;
      left: 50%;
      transform: translateX(-50%);
      width: 32px;
      height: 32px;
      border-radius: 50%;
      border: 4px solid #6a0b0b;
      box-shadow: 2px 2px 0 #000 inset, 3px 3px 0 #3e1a1a;
      text-align: center;
      color: #ffd700e4;
      text-shadow: 1px 1px 0 #000;
      font-weight: 700;
      font-size: 18px;
    }


    .grid-description {
        display: grid;
        padding: 0px;
        margin: -10px 0px 0px 10px; 
        grid-template-columns: 1fr, 1fr;
    }
    .form-quest-type {
      font-size: 24px; 
    }
    .form-danger-level {
      font-size: 22px;
    }
    .form-location {
      font-size: 24px;
      text-align: right;
      white-space: nowrap;
      overflow: hidden;
      grid-column: 2 / 2;
      grid-row: 1 / 2;
    }
    .form-reward {
      font-size: 22px;
      text-align: right;
      white-space: nowrap;
      overflow: hidden;
    }
    .form-reward img {
      position: relative;
      top: 2px;
      width: 20px;
      height: 20px;
    }

    .form:hover {
      transform: translate(0px, -4px);
      box-shadow: 6px 6px 0 #231302db, inset 0 0 0 2px #e0b973;
    }

    .form-actions {
      position: absolute;
      grid-area: actions;
      justify-self: center;
      margin-top: 66px;
      display: flex;
      gap: 6px;
    }

      .form-actions button {
        font-family: 'VT323', monospace;
        border: 2px solid #6a3d12;
        background: #2f1b0a;
        box-shadow: 2px 2px 0 #000;
        display: flex;
        align-items: center;
        justify-content: center;
        width: 70px;
        height: 26px;
        color: white;
        padding: 4px 0;
        font-size: 16px;
      }

        .form-actions button:active {
          transform: translate(2px, 2px);
          box-shadow: 0 0 0 #000;
        }

    .change-btn:hover {
      background: #5abb04;
    }

    .delete-btn:hover {
      background: #bd0000;
    }

    .coin {
      display: inline-block;
      width: 16px;
      height: 16px;
      background: radial-gradient(circle at 30% 30%, #ffeb6a, #d4a017);
      border: 2px solid #b8860b;
      border-radius: 50%;
    }
  </style>
