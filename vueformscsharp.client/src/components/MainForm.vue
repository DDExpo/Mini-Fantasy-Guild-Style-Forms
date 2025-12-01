<script setup lang="ts">
  import { computed, ref, watch } from "vue";

  import { useRouter } from 'vue-router'

  import type { Form, GuildFormProps } from "@/types/form";
  import { validateForm } from "@/validation/validateForm";
  import { forms, guildForm, EmptyForm, OptionsMainForm } from "@/store/forms";
  import { dangerColor } from "@/helpers/uiHelpers";
  import { submitFormApi } from "@/api/forms";

  const router = useRouter()
  const props = defineProps<GuildFormProps>();

  const errors = ref<Record<string, string>>({})
  const buttonText = computed(() => props.id ? 'Update Sroll' : 'Post Sroll');

  watch(
    () => props.id,
    (id) => {
      if (!id) {
        Object.assign(guildForm, EmptyForm);
        return;
      }

      const index = forms.value.findIndex(f => f.id === id);
      if (index !== -1) {
        Object.assign(guildForm, forms.value[index]!.data);
      }
    },
    { immediate: true }
  );

  async function submitForm() {
    errors.value = validateForm(guildForm);
    if (Object.keys(errors.value).length > 0) return;

    try {
        const result = await submitFormApi(props.id, guildForm);

        if (props.id) {
           const index = forms.value.findIndex(f => f.id === props.id);
           if (index !== -1) forms.value[index]!.data = { ...guildForm };
        } else {
        forms.value.push({ id: result.toString(), data: { ...guildForm } });
        }

        router.push('/');
    } catch (err) {
        alert('Failed to submit Quest: ' + err);
    }
  }

</script>

<template>
  <link href="https://fonts.googleapis.com/css2?family=Press+Start+2P&display=swap" rel="stylesheet">
  <div class="page">

    <form @submit.prevent="submitForm" class="guild-board">

      <h1 class="guild-title">
        <span class="emoji">📜</span> Guild Assignment
        <span class="text">Scroll</span>
      </h1>


      <div class="guild-section">
        <label for="questName">Quest Name</label>
        <input id="questName" :class="{ 'input-error': errors.questName }"
               type="text"
               v-model="guildForm.questName"
               placeholder="Ex: Slay the Shadow Wolf..." />
        <p v-if="errors.questName" class="error-text">
          {{ errors.questName }}
        </p>
      </div>

      <div class="guild-section">
        <label for="questType">Quest Type</label>
        <select id="questType" v-model="guildForm.questType">
          <option value="Monster Hunt">Monster Hunt</option>
          <option value="Escort Mission">Escort Mission</option>
          <option value="Delivery Run">Delivery Run</option>
          <option value="Investigation">Investigation</option>
          <option value="Defense / Hold Position">Defense / Hold Position</option>
          <option value="Extraction / Rescue">Extraction / Rescue</option>
          <option value="Exploration / Mapping">Exploration / Mapping</option>
          <option value="Assassination">Assassination</option>
          <option value="Resource Gathering">Resource Gathering</option>
          <option value="Dungeon Raid">Dungeon Raid</option>
        </select>
      </div>

      <div class="guild-section">
        <label for="dangerLevel">Danger Level</label>
        <input id="dangerLevel"
               type="range"
               min="1"
               max="10"
               v-model.number="guildForm.dangerLevel" />
        <span class="danger-display"
              :style="{ color: dangerColor(guildForm.dangerLevel) }">
           <span class="emoji">⚔️</span>{{guildForm.dangerLevel }}/10</span>
      </div>

      <div class="guild-section">
        <label for="location">Region / Location</label>
        <input id="location" :class="{ 'input-error': errors.location }"
               type="text"
               v-model="guildForm.location"
               placeholder="Ex: Blackwood Forest" />
        <p v-if="errors.location" class="error-text">
          {{ errors.location }}
        </p>
      </div>

      <div class="guild-section">
        <label for="reward">Gold Reward</label>
        <input id="reward" :class="{ 'input-error': errors.rewardGold }"
               type="text"
               inputmode="numeric"
               pattern="[0-9]*"
               v-model="guildForm.rewardGold"
               placeholder="Ex: 500" />
        <p v-if="errors.rewardGold" class="error-text">
          {{ errors.rewardGold }}
        </p>
      </div>

      <div class="guild-section">
        <label for="deadline">Deadline</label>
        <input id="deadline"
               type="text"
               v-model="guildForm.deadline" />
      </div>

      <textarea id="questDetails"
                v-model="guildForm.questDetails"
                placeholder="Describe the mission, rumors, warnings..."
                class="quest-textarea"></textarea>

      <div class="guild-requirements">
        <div class="req-item" v-for="(opt, index) in OptionsMainForm" :key="opt.id">
          <input type="checkbox"
                 :id="'req-' + opt.id"
                 v-model="guildForm.requirements[index]" />
          <span :for="'req-' + opt.id">{{ opt.label }}</span>
        </div>
      </div>

      <div class="guild-color">
        <label for="questColor">Stamp Color</label>
        <div class="guild-color-picker">
          <input type="color" id="questColor" v-model="guildForm.color">
          <span class="color-value">{{ guildForm.color }}</span>
        </div>
      </div>

      <div class="guild-section checkbox-main">
        <div class="guild-oath">
          <input type="checkbox"
                 v-model="guildForm.acceptedGuildCode"
                 id="guildRules">
          <label for="guildRules">I swear to honor the Guild Code</label>
        </div>
          <p v-if="errors.acceptedGuildCode" class="error-text">
            {{ errors.acceptedGuildCode }}
          </p>
      </div>

      <div class="guild-controls">
        <button type="submit"> {{ buttonText }}</button>
        <button type="button" @click="() => { router.push('/');}">Discard Scroll</button>
      </div>

    </form>
  </div>
</template>

<style scoped>
  .input-error {
    border: 2px solid #8b0000;
    background: #f5d2c8;
  }
  .error-text {
    font-size: 10px;
    margin-top: 4px;
    color: #7c0000
  }
  .page {
    display: flex;
    justify-content: center;
    padding: 60px 0;
    overflow-y: auto;
    height: 100vh;
  }

  .guild-board {
    width: 100%;
    max-width: 700px;
    background: #e8d8b0;
    border: 6px solid #7a5a2f;
    padding: 24px 32px 40px 32px;
    color: #3b2816;
    margin: auto;
    font-family: 'Press Start 2P', monospace;
    box-shadow: 8px 8px 0px 0px rgba(0, 0, 0, 0.7);
  }

  .guild-title {
    width: 100%;
    text-align: center;
    font-size: 24px;
    margin-bottom: 24px;
    color: rgb(211, 177, 26);
  }
    .guild-title .text {
      display: block;
      margin-left: 40px;
    }
    .guild-title .emoji {
      position: relative;
      top: -5px;
    }

  label {
    color: #070402;
  } 

  .guild-section {
    margin-bottom: 16px;
    display: flex;
    flex-direction: column;
    gap: 8px;
  }
  .guild-requirements {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 8px;
    background: #f3e6c8;
    border: 2px dashed #7a5a2f;
    padding: 8px;
  }

  .guild-color {
      margin-top: 16px;
  }

  .guild-color-picker {
    display: flex;
    align-items: center;
    gap: 8px;
    margin-top: 4px;
  }
    .guild-color-picker input[type="color"] {
      width: 30px;
      height: 30px;
      padding: 0;
      border: 3px solid #5b3a1c;
      box-shadow: inset 2px 2px 0 #3a240f;
      -webkit-appearance: none;
    }

      .guild-color-picker input[type="color"]::-webkit-color-swatch-wrapper {
        padding: 0;
      }

      .guild-color-picker input[type="color"]::-webkit-color-swatch {
        border: none;
        border-radius: 0;
      }

    .guild-color-picker .color-value {
      font-size: 14px;
      color: #513d2bcc;
    }

  .req-item {
    display: flex;
    align-items: center;
    gap: 8px;
  }

  .req-item input {
    accent-color: #7a5a2f;
  }
  .req-item span {
    font-size: 13px;
  }

    .guild-section label {
      font-size: 14px;
    }

  .guild-section input,
  .guild-section select,
  .guild-section textarea {
    background: #f3e6c8;
    color: #3b2816;
    border: 2px solid #7a5a2f;
    font-family: inherit;
    font-size: 12px;
    padding: 8px 6px 6px 6px;
    outline: none;
  }
    .quest-textarea {
      width: 100%;
      min-height: 120px;
      resize: vertical;
      padding: 10px;
      background: #f3e6c8;
      color: #3b2816;
      border: 2px solid #7a5a2f;
      font-family: inherit;
      font-size: 12px;
    }

      .guild-section input:focus,
      .guild-section textarea:focus,
      .guild-section select:focus {
        border-color: #c49a3a;
        box-shadow: 0 0 0 2px #a07b43;
      }

  input[type="range"] {
    -webkit-appearance: none;
    width: 100%;
    height: 8px;
    background: #c49a3a;
    border-radius: 0;
    border: 2px solid #7a5a2f;
    outline: none;
  }

    input[type="range"]::-webkit-slider-thumb {
      -webkit-appearance: none;
      width: 14px;
      height: 14px;
      background: #3b2816;
      border: 2px solid #7a5a2f;
    }

    input[type="range"]::-moz-range-thumb {
      width: 14px;
      height: 14px;
      background: #3b2816;
      border: 2px solid #7a5a2f;
    }

  textarea {
    min-height: 80px;
    resize: none;
  }

  .danger-display {
    font-size: 14px;
    margin-top: 0px;
  }

  .danger-display .emoji {
      position: relative;
      right: 2px;
      top: -3px;
  }

  .checkbox-group {
    display: flex;
    align-items: center;
    gap: 8px;
    margin-top: 4px;
  }

  .checkbox-main {
    margin-top: 10px;
    display: flex;
    align-items: center;
    justify-content: flex-start;
    width: 100%;
    text-align: left;
  }

  .checkbox-main {
    width: 100%;
    justify-content: flex-start;
  }

  .guild-oath {
    display: flex;
    align-items: center;
    gap: 10px;
    padding: 6px 10px;
    border: 2px solid #7a5a2f;
    background: #f7e9c6;
    max-width: fit-content;
  }

  .guild-oath input {
    accent-color: #7a5a2f;
    width: 16px;
    height: 16px;
  }

  .guild-oath label {
    font-size: 12px;
  }

  .guild-controls {
    display: flex;
    gap: 12px;
    justify-content: space-between;
    margin-top: 20px;
  }

    .guild-controls button {
      background: #3e2714;
      border: 3px solid #5b3a1c;
      color: #ffcc66;
      font-family: inherit;
      font-size: 10px;
      padding: 10px 12px;
    }

      .guild-controls button:hover {
        background: #0d0803f2;
        border-color: #ffdb92;
      }

      .guild-controls button:active {
        transform: translateY(1px);
      }

</style>
