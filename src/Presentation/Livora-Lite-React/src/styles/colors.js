// Saasable UI Color Palette
export const colors = {
    // Primary palette - Saasable Primary Blue
    primary: {
        lighter: '#E0E0FF',
        light: '#BDC2FF',
        main: '#606BDF',
        dark: '#3944B8',
        darker: '#000668'
    },
    // Secondary palette
    secondary: {
        lighter: '#E0E0FF',
        light: '#C3C4E4',
        main: '#5A5C78',
        dark: '#43455F',
        darker: '#171A31'
    },
    // Success palette
    success: {
        lighter: '#C8FFC0',
        light: '#B6F2AF',
        main: '#22892F',
        dark: '#006E1C',
        darker: '#00390A'
    },
    // Warning palette
    warning: {
        lighter: '#FFEEE1',
        light: '#FFDCBE',
        main: '#AE6600',
        dark: '#8B5000',
        darker: '#4A2800'
    },
    // Error palette
    error: {
        lighter: '#FFEDEA',
        light: '#FFDAD6',
        main: '#DE3730',
        dark: '#BA1A1A',
        darker: '#690005'
    },
    // Info palette
    info: {
        lighter: '#D4F7FF',
        light: '#A1EFFF',
        main: '#008394',
        dark: '#006876',
        darker: '#00363E'
    },
    // Neutral/Gray palette
    grey: {
        50: '#FBF8FF',
        100: '#F5F2FA',
        200: '#EFEDF4',
        300: '#EAE7EF',
        400: '#E4E1E6',
        500: '#DBD9E0',
        600: '#C7C5D0',
        700: '#777680',
        800: '#46464F',
        900: '#1B1B1F'
    },
    // Text colors
    text: {
        primary: '#1B1B1F',
        secondary: '#46464F',
        disabled: '#777680'
    },
    // Background colors
    background: {
        default: '#FFFFFF',
        paper: '#FFFFFF',
        light: '#FBF8FF'
    },
    // Border and divider
    border: '#EFEDF4',
    divider: '#EFEDF4',
    // Common
    black: '#000000',
    white: '#FFFFFF',
    // Livora Legacy Colors (keep for compatibility)
    livora: {
        primary: '#0081a7',
        secondary: '#00afb9',
        accentLight: '#fed9b7',
        accentDanger: '#f07167',
        lightBg: '#fdfcdc'
    }
};
export const colorUtilities = {
    // Combine color with alpha
    withAlpha: (color, alpha) => {
        const hex = color.replace('#', '');
        const r = parseInt(hex.substring(0, 2), 16);
        const g = parseInt(hex.substring(2, 4), 16);
        const b = parseInt(hex.substring(4, 6), 16);
        return `rgba(${r}, ${g}, ${b}, ${alpha})`;
    },
    // Get shades of a color
    shade: (color, percent) => {
        const num = parseInt(color.replace('#', ''), 16);
        const amt = Math.round(2.55 * percent);
        const R = Math.max(0, Math.min(255, (num >> 16) + amt));
        const G = Math.max(0, Math.min(255, (num >> 8) + amt));
        const B = Math.max(0, Math.min(255, (num & 0xff) + amt));
        return '#' + (0x1000000 + (R < 16 ? 0 : 1) * R * 0x10000 + (G < 16 ? 0 : 1) * G * 0x100 + (B < 16 ? 0 : 1) * B).toString(16).slice(1);
    }
};
// CSS Variables configuration for quick use in stylesheets
export const getCSSVariables = () => {
    return `
    :root {
      /* Primary Colors */
      --primary-lighter: ${colors.primary.lighter};
      --primary-light: ${colors.primary.light};
      --primary-main: ${colors.primary.main};
      --primary-dark: ${colors.primary.dark};
      --primary-darker: ${colors.primary.darker};
      
      /* Secondary Colors */
      --secondary-lighter: ${colors.secondary.lighter};
      --secondary-light: ${colors.secondary.light};
      --secondary-main: ${colors.secondary.main};
      --secondary-dark: ${colors.secondary.dark};
      --secondary-darker: ${colors.secondary.darker};
      
      /* Success Colors */
      --success-lighter: ${colors.success.lighter};
      --success-light: ${colors.success.light};
      --success-main: ${colors.success.main};
      --success-dark: ${colors.success.dark};
      --success-darker: ${colors.success.darker};
      
      /* Warning Colors */
      --warning-lighter: ${colors.warning.lighter};
      --warning-light: ${colors.warning.light};
      --warning-main: ${colors.warning.main};
      --warning-dark: ${colors.warning.dark};
      --warning-darker: ${colors.warning.darker};
      
      /* Error Colors */
      --error-lighter: ${colors.error.lighter};
      --error-light: ${colors.error.light};
      --error-main: ${colors.error.main};
      --error-dark: ${colors.error.dark};
      --error-darker: ${colors.error.darker};
      
      /* Info Colors */
      --info-lighter: ${colors.info.lighter};
      --info-light: ${colors.info.light};
      --info-main: ${colors.info.main};
      --info-dark: ${colors.info.dark};
      --info-darker: ${colors.info.darker};
      
      /* Neutral Colors */
      --grey-50: ${colors.grey[50]};
      --grey-100: ${colors.grey[100]};
      --grey-200: ${colors.grey[200]};
      --grey-300: ${colors.grey[300]};
      --grey-400: ${colors.grey[400]};
      --grey-500: ${colors.grey[500]};
      --grey-600: ${colors.grey[600]};
      --grey-700: ${colors.grey[700]};
      --grey-800: ${colors.grey[800]};
      --grey-900: ${colors.grey[900]};
      
      /* Text Colors */
      --text-primary: ${colors.text.primary};
      --text-secondary: ${colors.text.secondary};
      --text-disabled: ${colors.text.disabled};
      
      /* Background Colors */
      --bg-default: ${colors.background.default};
      --bg-paper: ${colors.background.paper};
      --bg-light: ${colors.background.light};
      
      /* Border Colors */
      --border-color: ${colors.border};
      --divider-color: ${colors.divider};
      
      /* Legacy Livora Colors */
      --livora-primary: ${colors.livora.primary};
      --livora-secondary: ${colors.livora.secondary};
      --livora-accent-light: ${colors.livora.accentLight};
      --livora-accent-danger: ${colors.livora.accentDanger};
      --livora-light-bg: ${colors.livora.lightBg};
    }
  `;
};
