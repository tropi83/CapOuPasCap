using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CapOuPasCap.Models.Classes;
using CapOuPasCap.Models.DataAccess;

namespace CapOuPasCap.Models
{
    public class DataInitializer
    {
        public static async Task SeedData(DataBaseContext context)
        {
            // Ajoute les utilisateurs
            if (!context.Utilisateur.Any())
            {
                var File = System.IO.File.ReadAllText("Seeders/user.seeder.json");
                var Utilisateurs = JsonSerializer.Deserialize<List<Utilisateur>>(File);

                await context.Utilisateur.AddRangeAsync(Utilisateurs);
                await context.SaveChangesAsync();

                // Ajoute les défis
                if (!context.Defi.Any())
                {
                    var FileChallenge = System.IO.File.ReadAllText("Seeders/challenge.seeder.json");
                    var Defis = JsonSerializer.Deserialize<List<Defi>>(FileChallenge);

                    foreach (var Defi in Defis)
                    {
                        Random rnd = new Random();
                        int rndRangeUser = rnd.Next(0, Utilisateurs.Count());
                        Defi.Createur = Utilisateurs[rndRangeUser];
                    }
                 
                    await context.Defi.AddRangeAsync(Defis);
                    await context.SaveChangesAsync();

                    // Ajoute les comentaires
                    if (!context.Commentaire.Any())
                    {
                        Random rnd = new Random();
                        var FileComment = System.IO.File.ReadAllText("Seeders/comment.seeder.json");
                        var Commentaires = JsonSerializer.Deserialize<List<Commentaire>>(FileComment);

                        foreach (var Commentaire in Commentaires)
                        {

                            int rndRangeUser = rnd.Next(0, Utilisateurs.Count());
                            Commentaire.Createur = Utilisateurs[rndRangeUser];

                            int rndRangeChallenge = rnd.Next(0, Defis.Count());
                            Commentaire.DefiId = Defis[rndRangeChallenge].Id;

                            Commentaire.Texte = Commentaire.Texte;
                            Commentaire.DateDeCreation = Commentaire.DateDeCreation;

                        }

                        await context.Commentaire.AddRangeAsync(Commentaires);
                        await context.SaveChangesAsync();
                    }

                    // Ajoute les likes
                    if (!context.Like.Any())
                    {
                        foreach (var Defi in Defis)
                        {
                            Random rnd = new Random();
                            int rndRangeUser = rnd.Next(0, Utilisateurs.Count());
                            int rndLike = rnd.Next(0, 3);
                            var Likes = new List<Like>();
                            for (int i = 0; i < rndLike; i++)
                            {
                                rnd = new Random();
                                rndRangeUser = rnd.Next(0, Utilisateurs.Count());
                                int rndRangeChallenge = rnd.Next(0, Defis.Count());

                                Like like = new()
                                {
                                    UtilisateurId = Utilisateurs[rndRangeUser].Id,
                                    DefiId = Defis[rndRangeChallenge].Id,
                                };

                                Likes.Add(like);
                            }

                            await context.Like.AddRangeAsync(Likes);
                            await context.SaveChangesAsync();
                        }
                    }

                    // Ajoute les défis réalisés 
                    if (!context.DefiRealise.Any())
                    {
                        foreach (var Defi in Defis)
                        {
                            Random rnd = new Random();
                            int rndRangeUser = rnd.Next(0, Utilisateurs.Count());
                            int rndLike = rnd.Next(0, 3);
                            var DefiRealises = new List<DefiRealise>();
                            for (int i = 0; i < rndLike; i++)
                            {
                                rnd = new Random();
                                rndRangeUser = rnd.Next(0, Utilisateurs.Count());
                                int rndRangeChallenge = rnd.Next(0, Defis.Count());

                                DefiRealise defiRealise = new()
                                {
                                    UtilisateurId = Utilisateurs[rndRangeUser].Id,
                                    DefiId = Defis[rndRangeChallenge].Id,
                                };

                                DefiRealises.Add(defiRealise);
                            }

                            await context.DefiRealise.AddRangeAsync(DefiRealises);
                            await context.SaveChangesAsync();
                        }
                    }

                }

            }

        }

    }
}
