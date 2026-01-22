using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using SmartPantry.Api.Models;

// SOLUÇÃO PARA O ERRO DE GUID: Registra como o MongoDB deve tratar o ID
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var builder = WebApplication.CreateBuilder(args);

// Configuração do MongoDB
var mongoClient = new MongoClient("mongodb://localhost:27017");
var database = mongoClient.GetDatabase("DespensaDB");

// Registra a coleção para que o Controller possa usá-la
builder.Services.AddSingleton(database.GetCollection<PantryItem>("Itens"));

builder.Services.AddControllers();

// Configuração de CORS (Permite que o Vue acesse a API)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();

app.Run();