
export interface Environment {
    production: boolean;
    apiBaseUrl: string;
  }
  
  export const environment: Environment = {
    production: false,
    apiBaseUrl: 'https://localhost:7062'
  };
  