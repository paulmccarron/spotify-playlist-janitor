export const isAuthDataValid = (token?: string, expiresOn?: number) => {
  const now = Math.floor(new Date(Date.now()).getTime() / 1000);

  return (
    typeof token === "string" &&
    typeof expiresOn === "number" &&
    now < expiresOn
  );
};
